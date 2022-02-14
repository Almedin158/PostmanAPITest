using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftraySolutions.Model
{
    internal class PostResponse
    {
        public Collection collection { get; set; }

        //public PostResponse(string _name, string _schema, string _uid, string _id, bool _item = false)
        //{
        //    collection = new Collection();
        //    collection.info = new Info();
        //    collection.info.name = _name;
        //    collection.info.schema = _schema;
        //    if (_item == true)
        //    {
        //        collection.item = new List<Item>();
        //    }
        //    collection.id= _id;
        //    collection.uid = _uid;
        //}
    }
}
