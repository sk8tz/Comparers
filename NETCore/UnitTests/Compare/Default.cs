﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.Comparers;
using Nito.EqualityComparers;

namespace Compare_
{
    [TestClass]
    public class _Default
    {
        [TestMethod]
        public void IsEquivalentToComparerDefault()
        {
            var list1 = new[] { 3, 5, 4, 2, 6 }.ToList();
            var list2 = new[] { 3, 5, 4, 2, 6 }.ToList();
            list1.Sort();
            list2.Sort(ComparerBuilder.For<int>().Default());
            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void UsesSequenceComparerForSequences()
        {
            var three = new[] { 3 };
            var four = new[] { 4 };
            var five = new[] { 5 };
            var list1 = new[] { three, five, four }.ToList();
            var list2 = new[] { three, five, four }.ToList();
            var comparer1 = ComparerBuilder.For<int>().Default().Sequence();
            var comparer2 = ComparerBuilder.For<int[]>().Default();
            list1.Sort(comparer1);
            list2.Sort(comparer2);
            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void NullIsLessThanValue()
        {
            var list = new int?[] { 3, null, 4, 2, 6 }.ToList();
            list.Sort(ComparerBuilder.For<int?>().Default());
            CollectionAssert.AreEqual(list, new int?[] { null, 2, 3, 4, 6 });
        }

        [TestMethod]
        public void NullSequenceIsLessThanValuesAndEmptySequence()
        {
            var none = new int[0];
            var five = new[] { 5 };
            var list = new[] { five, none, null }.ToList();
            list.Sort(ComparerBuilder.For<int[]>().Default());
            CollectionAssert.AreEqual(list, new[] { null, none, five });
        }

        [TestMethod]
        public void NullIsEqualToNull()
        {
            var comparer = ComparerBuilder.For<int?>().Default();
            Assert.IsTrue(comparer.Compare(null, null) == 0);
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.AreEqual(comparer.GetHashCode((object)null), comparer.GetHashCode((object)null));
            Assert.AreEqual(comparer.GetHashCode((int?)null), comparer.GetHashCode((int?)null));
        }

        [TestMethod]
        public void NullSequenceIsEqualToNullSequence()
        {
            var comparer = ComparerBuilder.For<int[]>().Default();
            Assert.IsTrue(comparer.Compare(null, null) == 0);
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.AreEqual(comparer.GetHashCode((object)null), comparer.GetHashCode((object)null));
            Assert.AreEqual(comparer.GetHashCode((int[])null), comparer.GetHashCode((int[])null));
        }

        [TestMethod]
        public void ImplementsGetHashCode()
        {
            var comparer = ComparerBuilder.For<int?>().Default();
            var bclDefault = EqualityComparer<int?>.Default;
            Assert.AreEqual(comparer.GetHashCode(13), comparer.GetHashCode(13));
            Assert.AreEqual(bclDefault.GetHashCode(7) == bclDefault.GetHashCode(13), comparer.GetHashCode(7) == comparer.GetHashCode(13));
        }

        [TestMethod]
        public void ImplementsGetHashCodeForNull()
        {
            var comparer = ComparerBuilder.For<int?>().Default();
            Assert.AreEqual(comparer.GetHashCode((object)null), comparer.GetHashCode((object)null));
            Assert.AreEqual(comparer.GetHashCode((int?)null), comparer.GetHashCode((int?)null));
        }

        [TestMethod]
        public void UsesSequenceComparerForEnumerables()
        {
            var three = new[] { 3 };
            var four = new[] { 4 };
            var five = new[] { 5 };
            var list1 = new[] { three, five, four }.ToList();
            var list2 = new[] { three, five, four }.ToList();
            var comparer1 = ComparerBuilder.For<int>().Default().Sequence();
            var comparer2 = ComparerBuilder.For<IEnumerable<int>>().Default();
            list1.Sort(comparer1);
            list2.Sort(comparer2);
            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void DefaultForString_IsDefaultComparer()
        {
            // Ensure string default comparer is not a sequence comparer over chars.
            Assert.AreSame(Nito.Comparers.Util.DefaultComparer<string>.Instance, ComparerBuilder.For<string>().Default());
        }
    }
}
