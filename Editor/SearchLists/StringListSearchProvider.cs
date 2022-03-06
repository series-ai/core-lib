using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Padoru.Core.Editor
{
    public class StringListSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private List<string> items;
        private string listTitle;
        private Action<string> onSetEntryCallback;

        public StringListSearchProvider(string listTitle, List<string> items, Action<string> onSetEntryCallback)
        {
            items.Sort();

            this.items = items;
            this.listTitle = listTitle;
            this.onSetEntryCallback = onSetEntryCallback;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchList = new List<SearchTreeEntry>();
            searchList.Add(new SearchTreeGroupEntry(new GUIContent(listTitle), 0));

            for (int i = 0; i < items.Count; i++)
            {
                var entry = new SearchTreeEntry(new GUIContent(items[i]));
                entry.level = 1;
                entry.userData = items[i];
                searchList.Add(entry);
            }

            return searchList;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            onSetEntryCallback?.Invoke((string)SearchTreeEntry.userData);
            return true;
        }
    }
}