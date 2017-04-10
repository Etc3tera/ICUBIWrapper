using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestDLL
{

    class Program
    {
        [DllImport("ICUBIWrapperCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetWordBoundary(IntPtr unicodeBytes, IntPtr result);
        [DllImport("ICUBIWrapperCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Sum(int a,int b);

        [DllImport("ICUBIWrapperCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Foo(IntPtr test);
        static void Main(string[] args)
        {
            //Console.WriteLine(GetWordBoundary("คนไทย"));

            string input_string = "คนไทยรักกัน";
            byte[] input_bytes = Encoding.UTF8.GetBytes(input_string);

            int[] output = new int[100];
            IntPtr test = Marshal.AllocHGlobal(400);
            IntPtr input = Marshal.AllocHGlobal(input_bytes.Length * 2);
            Marshal.Copy(input_bytes, 0, input, input_bytes.Length);

            int len = GetWordBoundary(input, test);

            Marshal.Copy(test, output, 0, len);
            for (int i = 0; i < len; i++)
            {
                Console.Write(output[i] + " ");
            }
            Marshal.FreeHGlobal(test);
        }
    }
}
