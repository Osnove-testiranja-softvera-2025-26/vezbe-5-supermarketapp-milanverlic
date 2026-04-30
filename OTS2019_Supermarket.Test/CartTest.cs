using NUnit.Framework;
using OTS_Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS_Supermarket.Test
{
    [TestFixture]
    public class CartTest
    {
        [Test]
        public void AddOneToCart_ShouldAddItemToCart_Success()
        {
            // ARRANGE
            Cart cart = new Cart();
            Monitor monitor = new Monitor();

            // ACT
            cart.AddOneToCart(monitor);

            // ASSERT
            Assert.That(cart.Size, Is.EqualTo(1));
            Assert.That(cart.Amount, Is.EqualTo(100));
        }

        [Test]
        public void AddMultipleToCart_ShouldAddItemsAndUpdateCounters()
        {
            // ARRANGE
            Cart cart = new Cart();
            Monitor monitor = new Monitor();

            // ACT
            cart.AddMultipleToCart(monitor, 3);

            // ASSERT
            Assert.That(cart.Size, Is.EqualTo(3));
            Assert.That(cart.Amount, Is.EqualTo(300));
            Assert.That(cart.Monitor_counter, Is.EqualTo(3));
        }

        [Test]
        public void AddToCart_ExceedMax_ThrowsException()
        {
            // ARRANGE
            Cart cart = new Cart();
            Monitor monitor = new Monitor();

            // fill cart to max
            cart.AddMultipleToCart(monitor, 10);

            // ACT
            var ex = Assert.Throws<Exception>(() => cart.AddOneToCart(monitor));

            // ASSERT
            Assert.That(ex.Message, Is.EqualTo("Number of items in cart must be 10 or less!"));
        }

        [Test]
        public void DeleteAll_OnNonEmpty_ClearsCart()
        {
            // ARRANGE
            Cart cart = new Cart();
            Keyboard kb = new Keyboard();
            Computer pc = new Computer();

            cart.AddOneToCart(kb);
            cart.AddOneToCart(pc);

            // ACT
            cart.DeleteAll();

            // ASSERT
            Assert.That(cart.Size, Is.EqualTo(0));
            Assert.That(cart.Items.Count, Is.EqualTo(0));
            Assert.That(cart.Keyboard_counter, Is.EqualTo(0));
            Assert.That(cart.Computer_counter, Is.EqualTo(0));
        }

        [Test]
        public void DeleteAll_OnEmpty_ThrowsException()
        {
            // ARRANGE
            Cart cart = new Cart();

            // ACT
            var ex = Assert.Throws<Exception>(() => cart.DeleteAll());

            // ASSERT
            Assert.That(ex.Message, Is.EqualTo("Cannot restore empty cart!"));
        }

        [Test]
        public void Print_OnNonEmpty_ReturnsConcatenatedString()
        {
            // ARRANGE
            Cart cart = new Cart();
            Computer pc = new Computer();

            cart.AddOneToCart(pc);

            // ACT
            string printed = cart.Print();

            // ASSERT
            Assert.That(printed, Does.Contain("Computer"));
            Assert.That(printed, Does.Contain("1200"));
        }

        [Test]
        public void Print_OnEmpty_ThrowsException()
        {
            // ARRANGE
            Cart cart = new Cart();

            // ACT
            var ex = Assert.Throws<Exception>(() => cart.Print());

            // ASSERT
            Assert.That(ex.Message, Is.EqualTo("Cannot print empty cart!"));
        }

        [Test]
        public void Calculate_WrongDateFormat_ThrowsException()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.Budget = 10000; // enough budget
            cart.AddOneToCart(new Monitor());

            // ACT
            var ex = Assert.Throws<Exception>(() => cart.Calculate("01-01-2025"));

            // ASSERT
            Assert.That(ex.Message, Is.EqualTo("Wrong date format! Date must be in format yyyy-MM-dd"));
        }

        [Test]
        public void Calculate_TodayDate_ThrowsException()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.Budget = 10000;
            cart.AddOneToCart(new Monitor());

            string today = DateTime.Today.ToString("yyyy-MM-dd");

            // ACT
            var ex = Assert.Throws<Exception>(() => cart.Calculate(today));

            // ASSERT
            Assert.That(ex.Message, Is.EqualTo("Date of delivery can't be today's date!"));
        }

        [Test]
        public void Calculate_NotEnoughBudget_ThrowsException()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.Budget = 100; // less than any single expensive item
            cart.AddOneToCart(new Computer()); // price 1200

            string date = DateTime.Today.AddDays(5).ToString("yyyy-MM-dd");

            // ACT
            var ex = Assert.Throws<Exception>(() => cart.Calculate(date));

            // ASSERT
            Assert.That(ex.Message, Is.EqualTo("Not enough budget!"));
        }
    }
}
