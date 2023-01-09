using System;
using System.Collections.Generic;

namespace ZOS
{
    public class LRUTDemo
    {
        public void Run()
        {
            LRUCacheT lRUCache = new LRUCacheT(5);
            lRUCache.Put("001", "User1 - Message");
            lRUCache.Put("002", "User2 - Message");
            lRUCache.Put("003", "User3 - Message");
            lRUCache.Put("004", "User4 - Message");
            lRUCache.Put("005", "User5 - Message");
            lRUCache.Get("002");
            lRUCache.Put("004", "User4 - NewMessage");
            lRUCache.Put("006", "User6 - UpdateMessage");
            Console.WriteLine(lRUCache.Get("001"));
            Console.WriteLine(lRUCache.Get("006"));
        }
    }

    public class Node
    {
        public string key;
        public string value;
        public Node pre;
        public Node next;
        public Node(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
 
    public class LRUCacheT
    {
        private Node head;
        private Node end;
        private int limit;
        private Dictionary<string, Node> dict;
        
        public LRUCacheT(int limit)
        {
            this.limit = limit;
            dict = new Dictionary<string, Node>();
        }
 
        public string Get(string key)
        {
            Node node = null;
            if(!dict.TryGetValue(key,out node))
            {
                return null;
            }
            RefreshNode(node);
            return node.value;
        }
 
        public void Put(string key, string value)
        {
            Node node = null;
            if(!dict.TryGetValue(key, out node))
            {
                if(dict.Count >= limit)
                {
                    string oldKey = RemoveNode(head);
                    dict.Remove(oldKey);
                }
                node = new Node(key, value);
                AddNode(node);
                dict.Add(key, node);
            }
            else
            {
                node.value = value;
                RefreshNode(node);
            }            
        }
 
        public void Remove(string key)
        {
            Node node = null;
            if(dict.TryGetValue(key, out node))
            {
                RemoveNode(node);
                dict.Remove(key);
            }
        }
 
        private void RefreshNode(Node node)
        {
            if (node == end)
                return;
            RemoveNode(node);
            AddNode(node);
        }
 
        private void AddNode(Node node)
        {
            if (end != null)
            {
                end.next = node;
                node.pre = end;
                node.next = null;
            }
            end = node;
            if (head == null)
                head = node;
        }
 
        private string RemoveNode(Node node)
        {
            if (node == head && node == end)
            {
                head = null;
                end = null;
            }else if (node == end)
            {
                end = end.pre;
                end.next = null;
            }else if (node == head)
            {
                head = head.next;
                head.pre = null;
            }
            else
            {
                node.pre.next = node.next;
                node.next.pre = node.pre;
            }
            return node.key;
        }
    }
}
