using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oil.AppCode
{
    public class PageItem<T>
    {

            public int code { get; set; }
            public int count { get; set; }
            public List<T> data { get; set; }
            public int msg { get; set; }

    }
}