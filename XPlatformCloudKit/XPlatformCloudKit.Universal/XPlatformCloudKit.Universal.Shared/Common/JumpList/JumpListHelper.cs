using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Globalization.Collation;

namespace JumpListSample.Common.JumpList
{
    public static class JumpListHelper
    {
        /// <summary>
        /// Groups and sorts into a list of group lists based on a selector.
        /// </summary>
        /// <typeparam name="TSource">Type of the items in the list.</typeparam>
        /// <typeparam name="TSort">Type of value returned by sortSelector.</typeparam>
        /// <typeparam name="TGroup">Type of value returned by groupSelector.</typeparam>
        /// <param name="source">List to be grouped and sorted</param>
        /// <param name="sortSelector">A selector that provides the value that items will be sorted by.</param>
        /// <param name="groupSelector">A selector that provides the value that items will be grouped by.</param>
        /// <param name="isSortDescending">Value indicating to sort groups in reverse. Items in group will still sort ascending.</param>
        /// <returns>A list of JumpListGroups.</returns>
        public static List<JumpListGroup<TSource>> ToGroups<TSource, TSort, TGroup>(
            this IEnumerable<TSource> source, Func<TSource, TSort> sortSelector,
            Func<TSource, TGroup> groupSelector, bool isSortDescending = false)
        {
            var groups = new List<JumpListGroup<TSource>>();

            // Group and sort items based on values returned from the selectors
            var query = from item in source
                        orderby groupSelector(item), sortSelector(item)
                        group item by groupSelector(item) into g
                        select new { GroupName = g.Key, Items = g };

            // For each group generated from the query, create a JumpListGroup
            // and fill it with its items
            foreach (var g in query)
            {
                JumpListGroup<TSource> group = new JumpListGroup<TSource>();
                group.Key = g.GroupName;
                foreach (var item in g.Items)
                    group.Add(item);

                if (isSortDescending)
                    groups.Insert(0, group);
                else
                    groups.Add(group);
            }

            return groups;
        }

        /// <summary>
        /// Groups and sorts into a list of alpha groups based on a string selector.
        /// </summary>
        /// <typeparam name="TSource">Type of the items in the list.</typeparam>
        /// <param name="source">List to be grouped and sorted.</param>
        /// <param name="selector">A selector that will provide a value that items to be sorted and grouped by.</param>
        /// <returns>A list of JumpListGroups.</returns>
        public static List<JumpListGroup<TSource>> ToAlphaGroups<TSource>(
            this IEnumerable<TSource> source, Func<TSource, string> selector)
        {
            // Get the letters representing each group for current language using CharacterGroupings class
            var characterGroupings = new CharacterGroupings();

            // Create dictionary for the letters and replace '...' with proper globe icon
            var keys = characterGroupings.Where(x => x.Label.Count() >= 1)
                .Select(x => x.Label)
                .ToDictionary(x => x);
            keys["..."] = "\uD83C\uDF10";

            // Create groups for each letters
            var groupDictionary = keys.Select(x => new JumpListGroup<TSource>() { Key = x.Value })
                .ToDictionary(x => (string)x.Key);

            // Sort and group items into the groups based on the value returned by the selector
            var query = from item in source
                        orderby selector(item)
                        select item;

            foreach (var item in query)
            {
                var sortValue = selector(item);
                groupDictionary[keys[characterGroupings.Lookup(sortValue)]].Add(item);
            }

            return groupDictionary.Select(x => x.Value).ToList();
        }
    }
}