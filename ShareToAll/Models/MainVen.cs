using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareToAll.Models
{
    public class MainVen
    {
        public class Meta
        {
            public int code { get; set; }
            public string errorType { get; set; }
            public string errorDetail { get; set; }
        }

        public class Item
        {
            public int unreadCount { get; set; }
        }

        public class Notification
        {
            public string type { get; set; }
            public Item item { get; set; }
        }

        public class Contact
        {
        }

        public class Location
        {
            public string address { get; set; }
            public string crossStreet { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
            public string postalCode { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
        }

        public class Category
        {
            public string id { get; set; }
            public string name { get; set; }
            public string pluralName { get; set; }
            public string shortName { get; set; }
            public Icon icon { get; set; }
            public List<string> parents { get; set; }
            public bool primary { get; set; }
        }

        public class Icon
        {
            public string prefix { get; set; }
            // public string sizes { get; set; }
            public string name { get; set; }

        }


        public class Stats
        {
            public int checkinsCount { get; set; }
            public int usersCount { get; set; }
            public int tipCount { get; set; }
        }

        public class Likes
        {
            public int count { get; set; }
            public List<object> groups { get; set; }
        }

        public class BeenHere
        {
            public int count { get; set; }
        }

        public class Group
        {
            public string type { get; set; }
            public string name { get; set; }
            public int count { get; set; }
            public List<object> items { get; set; }
        }

        public class Photos
        {
            public int count { get; set; }
            public List<Group> groups { get; set; }
            public string summary { get; set; }
        }

        public class HereNow
        {
            public int count { get; set; }
            public string summary { get; set; }
            public List<object> groups { get; set; }
        }

        public class Tips
        {
            public int count { get; set; }
        }

        public class Contact2
        {
            public string twitter { get; set; }
            public string facebook { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public string firstName { get; set; }
            public string photo { get; set; }
            public Tips tips { get; set; }
            public string gender { get; set; }
            public string homeCity { get; set; }
            public string bio { get; set; }
            public Contact2 contact { get; set; }
        }

        public class Mayor
        {
            public int count { get; set; }
            public User user { get; set; }
        }

        public class Likes2
        {
            public int count { get; set; }
            public List<object> groups { get; set; }
        }

        public class Todo
        {
            public int count { get; set; }
        }

        public class Done
        {
            public int count { get; set; }
        }

        public class Tips3
        {
            public int count { get; set; }
        }

        public class Group3
        {
            public string type { get; set; }
            public int count { get; set; }
            public List<object> items { get; set; }
        }

        public class Lists
        {
            public List<Group3> groups { get; set; }
        }

        public class Contact3
        {
            public string twitter { get; set; }
            public string facebook { get; set; }
        }

        public class User2
        {
            public string id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string photo { get; set; }
            public Tips3 tips { get; set; }
            public Lists lists { get; set; }
            public string gender { get; set; }
            public string homeCity { get; set; }
            public string bio { get; set; }
            public Contact3 contact { get; set; }
        }

        public class Item2
        {
            public string id { get; set; }
            public int createdAt { get; set; }
            public string text { get; set; }
            public Likes2 likes { get; set; }
            public Todo todo { get; set; }
            public Done done { get; set; }
            public User2 user { get; set; }
        }

        public class Group2
        {
            public string type { get; set; }
            public string name { get; set; }
            public int count { get; set; }
            public List<Item2> items { get; set; }
        }

        public class Tips2
        {
            public int count { get; set; }
            public List<Group2> groups { get; set; }
        }

        public class Listed
        {
            public int count { get; set; }
        }

        public class Todos
        {
            public int count { get; set; }
            public List<object> items { get; set; }
        }

        public class Venue
        {
            public string id { get; set; }
            public string name { get; set; }
            public Contact contact { get; set; }
            public Location location { get; set; }
            public List<Category> categories { get; set; }
            public bool verified { get; set; }
            public Stats stats { get; set; }
            public Likes likes { get; set; }
            public BeenHere beenHere { get; set; }
            //public List<object> specials { get; set; }
            public Photos photos { get; set; }
            public HereNow hereNow { get; set; }
            public int createdAt { get; set; }
            public Mayor mayor { get; set; }
            public Tips2 tips { get; set; }
            public List<object> tags { get; set; }
            public string shortUrl { get; set; }
            public string canonicalUrl { get; set; }
            public string timeZone { get; set; }
            public Listed listed { get; set; }
            public Todos todos { get; set; }
        }

        public class Response
        {
            //public Venue[] venues { get; set; }
            public List<Venue> venues { get; set; }
        }

        public class RootObject
        {
            public Meta meta { get; set; }
            //public Venue[] results { get; set; }
            //public List<Notification> notifications { get; set; }
            public Response response { get; set; }
        }
    }
}
