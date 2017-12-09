using LuaInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parabollian
{
    public class OLua
    {
        public Lua Lua { get; set; }

        public OLua()
        {
            Lua = new Lua();

            /*Register your command here*/
            Lua.RegisterFunction("sin", this, typeof(OLua).GetMethod("sin"));
            Lua.RegisterFunction("cos", this, typeof(OLua).GetMethod("cos"));
            Lua.RegisterFunction("tg", this, typeof(OLua).GetMethod("tg"));
            Lua.RegisterFunction("ctg", this, typeof(OLua).GetMethod("ctg"));
        }

        public float sin(double x) { return (float)Math.Sin(x); }
        public float cos(double x) { return (float)Math.Cos(x); }
        public float tg(double x) { return (float)Math.Tan(x); }
        public float ctg(double x) { return (float)Math.Atan(x/1); }
    }
}
