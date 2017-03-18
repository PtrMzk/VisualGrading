#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-18
// SmartSearch.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VisualGrading.Helpers;

#endregion

namespace VisualGrading.Search
{
    public class SmartSearch<T> where T : IIdentified
    {
        #region Fields

        private const char SPACE = ' ';
        private const string ID_PATTERN = @"\w+:\[\d+\]";
        private const char SEMICOLON = ':';
        private const string ID = "ID";

        #endregion

        #region Public Methods

        public HashSet<long> Search<T>(IList<T> objectsToSearch, string searchString) where T : IIdentified
        {
            var filteredIDs = new HashSet<long>();
            var searchWords = searchString.Split(SPACE);
            var filteredObjectsToSearch = new List<T>();
            var filteredSearchWords = new List<string>();
            var finalMatchingIDs = new HashSet<long>();

            if (objectsToSearch == null || objectsToSearch.Count == 0)
                return finalMatchingIDs;

            foreach (var searchWord in searchWords)
                if (Regex.IsMatch(searchWord, ID_PATTERN))
                {
                    var idSearchTerm = searchWord;
                    filteredIDs.UnionWith(GetFilteredObjectsByMatchingID(idSearchTerm, objectsToSearch));
                }
                else
                {
                    filteredSearchWords.Add(searchWord);
                }

            filteredObjectsToSearch = FilterObjectsToSearch(objectsToSearch, filteredIDs);

            var convertedStringsByObjectID = ConvertObjectsToStrings(filteredObjectsToSearch);

            finalMatchingIDs = GetMatchingIDs(filteredIDs, filteredSearchWords, convertedStringsByObjectID);

            return finalMatchingIDs;
        }

        #endregion

        #region Private Methods

        private Dictionary<long, string> ConvertObjectsToStrings<T>(IList<T> objectsToSearch) where T : IIdentified
        {
            var convertedStrings = new Dictionary<long, string>();
            var stringBuilder = new StringBuilder();

            foreach (var objectToSearch in objectsToSearch)
            {
                convertedStrings.Add(objectToSearch.ID, string.Empty);

                foreach (var property in typeof(T).GetProperties())
                {
                    if (property.Name.ToUpper().EndsWith(ID)) //skip ID properties
                        continue;

                    var propertyValue = property.GetValue(objectToSearch) != null
                        ? property.GetValue(objectToSearch).ToString()
                        : string.Empty;

                    if (stringBuilder.Length == 0)
                        stringBuilder.Append(propertyValue);
                    else
                        stringBuilder.AppendFormat(" {0}", propertyValue);
                }
                convertedStrings[objectToSearch.ID] = stringBuilder.ToString();
                stringBuilder.Clear();
            }
            return convertedStrings;
        }

        private List<T> FilterObjectsToSearch<T>(IList<T> objectsToSearch, HashSet<long> filteredIDs)
            where T : IIdentified
        {
            List<T> filteredObjectsToSearch;
            if (filteredIDs.Count > 0)
                filteredObjectsToSearch = objectsToSearch.Where(x => filteredIDs.Contains(x.ID)).ToList();
            else
                filteredObjectsToSearch = objectsToSearch.ToList();
            return filteredObjectsToSearch;
        }

        private HashSet<long> GetFilteredObjectsByMatchingID<T>(string idSearchTerm, IList<T> objectsToSearch)
            where T : IIdentified
        {
            long id;
            var semicolonIndex = idSearchTerm.IndexOf(SEMICOLON);
            var idString = idSearchTerm.Substring(semicolonIndex + 2, idSearchTerm.Length - semicolonIndex - 3);
            var idProperty = idSearchTerm.Substring(0, idSearchTerm.IndexOf(SEMICOLON));

            if (long.TryParse(idString, out id))
                return SearchForID(objectsToSearch, idProperty, id);

            return new HashSet<long>();
        }

        private HashSet<long> GetMatchingIDs(HashSet<long> filteredIDs,
            List<string> filteredSearchWords,
            Dictionary<long, string> convertedStringsByObjectID)
        {
            var matchingIDs = new HashSet<long>();

            if (filteredSearchWords.Count > 0)
                foreach (var kvp in convertedStringsByObjectID)
                {
                    var matchesAllSearchWords = true;

                    foreach (var searchWord in filteredSearchWords)
                        if (!string.IsNullOrWhiteSpace(searchWord))
                            if (kvp.Value.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) == -1)
                            {
                                matchesAllSearchWords = false;
                                break;
                            }

                    if (matchesAllSearchWords)
                        matchingIDs.Add(kvp.Key);
                }
            else if (filteredIDs.Count > 0 && filteredSearchWords.Count == 0)
                matchingIDs.UnionWith(filteredIDs);

            return matchingIDs;
        }

        private HashSet<long> SearchForID<T>(IList<T> objectsToSearch, string idProperty, long idToSearch)
            where T : IIdentified
        {
            var properties = typeof(T).GetProperties();
            var matchingIDs = new HashSet<long>();
            var objectContainsProperty = false;

            foreach (var property in properties)
                if (string.Equals(property.Name, idProperty, StringComparison.OrdinalIgnoreCase))
                {
                    objectContainsProperty = true;
                    break;
                }

            if (objectContainsProperty)
                foreach (var objectToSearch in objectsToSearch)
                {
                    //note that idValue can be a StudentID or a TestID on a Grade object, so this must use the property field that is passed
                    var idValue = (long) objectToSearch.GetType().GetProperty(idProperty).GetValue(objectToSearch, null);

                    if (idValue == idToSearch)
                        matchingIDs.Add(objectToSearch.ID);
                }

            return matchingIDs;
        }

        #endregion
    }
}