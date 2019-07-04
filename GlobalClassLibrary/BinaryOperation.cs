using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public static class BinaryOperation
    {
        //private const int bitsinbyte = 8;
        //private static readonly int paralleldegree;
        //private static readonly int uintsize;
        //private static readonly int bitsinuint;
        //static BinaryOperation()
        //    {
        //    paralleldegree = Environment.ProcessorCount;
        //    uintsize = sizeof(uint) / sizeof(byte); // really paranoid, uh ?
        //    bitsinuint = uintsize * bitsinbyte;
        //    }

        //public static byte[] Bin_And(this byte[] ba, byte[] bt)
        //    {
        //    int lenbig = Math.Max(ba.Length, bt.Length);
        //    int lensmall = Math.Min(ba.Length, bt.Length);
        //    byte[] result = new byte[lenbig];
        //    int ipar = 0;
        //    object o = new object();
        //    System.Action paction = delegate()
        //    {
        //        int actidx;
        //        lock (o)
        //            {
        //            actidx = ipar++;
        //            }
        //        unsafe
        //            {
        //            fixed (byte* ptres = result, ptba = ba, ptbt = bt)
        //                {
        //                uint* pr = (uint*)ptres;
        //                uint* pa = (uint*)ptba;
        //                uint* pt = (uint*)ptbt;
        //                pr += actidx; pa += actidx; pt += actidx;
        //                while (pr < ptres + lensmall)
        //                    {
        //                    *pr = (*pt & *pa);
        //                    pr += paralleldegree; pa += paralleldegree; pt += paralleldegree;
        //                    }
        //                }
        //            }
        //    };
        //    System.Action[] actions = new Action[paralleldegree];
        //    for (int i = 0; i < paralleldegree; i++)
        //        {
        //        actions[i] = paction;
        //        }
        //    Parallel.Invoke(actions);
        //    return result;
        //    }
            
        //public static byte[] Bin_Or(this byte[] ba, byte[] bt)
        //    {
        //    int lenbig = Math.Max(ba.Length, bt.Length);
        //    int lensmall = Math.Min(ba.Length, bt.Length);
        //    byte[] result = new byte[lenbig];
        //    int ipar = 0;
        //    object o = new object();
        //    System.Action paction = delegate()
        //        {
        //            int actidx;
        //            lock (o)
        //                {
        //                actidx = ipar++;
        //                }
        //            unsafe
        //                {
        //                fixed (byte* ptres = result, ptba = ba, ptbt = bt)
        //                    {
        //                    uint* pr = (uint*)ptres;
        //                    uint* pa = (uint*)ptba;
        //                    uint* pt = (uint*)ptbt;
        //                    pr += actidx; pa += actidx; pt += actidx;
        //                    while (pr < ptres + lensmall)
        //                        {
        //                        *pr = (*pt | *pa);
        //                        pr += paralleldegree; pa += paralleldegree; pt += paralleldegree;
        //                        }
        //                    uint* pl = ba.Length > bt.Length ? pa : pt;
        //                    while (pr < ptres + lenbig)
        //                        {
        //                        *pr = *pl;
        //                        pr += paralleldegree; pl += paralleldegree;
        //                        }
        //                    }
        //                }
        //        };
        //    System.Action[] actions = new Action[paralleldegree];
        //    for (int i = 0; i < paralleldegree; i++)
        //        {
        //        actions[i] = paction;
        //        }
        //    Parallel.Invoke(actions);

        //    return result;
        //    }
            
        //public static byte[] Bin_Xor(this byte[] ba, byte[] bt)
        //    {
        //    int lenbig = Math.Max(ba.Length, bt.Length);
        //    int lensmall = Math.Min(ba.Length, bt.Length);
        //    byte[] result = new byte[lenbig];
        //    int ipar = 0;
        //    object o = new object();
        //    System.Action paction = delegate()
        //        {
        //            int actidx;
        //            lock (o)
        //                {
        //                actidx = ipar++;
        //                }
        //            unsafe
        //                {
        //                fixed (byte* ptres = result, ptba = ba, ptbt = bt)
        //                    {
        //                    uint* pr = ((uint*)ptres) + actidx;
        //                    uint* pa = ((uint*)ptba) + actidx;
        //                    uint* pt = ((uint*)ptbt) + actidx;
        //                    while (pr < ptres + lensmall)
        //                        {
        //                        *pr = (*pt ^ *pa);
        //                        pr += paralleldegree; pa += paralleldegree; pt += paralleldegree;
        //                        }
        //                    uint* pl = ba.Length > bt.Length ? pa : pt;
        //                    while (pr < ptres + lenbig)
        //                        {
        //                        *pr = *pl;
        //                        pr += paralleldegree; pl += paralleldegree;
        //                        }
        //                    }
        //                }
        //        };
        //    System.Action[] actions = new Action[paralleldegree];
        //    for (int i = 0; i < paralleldegree; i++)
        //        {
        //        actions[i] = paction;
        //        }
        //    Parallel.Invoke(actions);

        //    return result;
        //    }
            
        //public static byte[] Bin_Not(this byte[] ba)
        //    {
        //    int len = ba.Length;
        //    byte[] result = new byte[len];
            
        //    int ipar = 0;
        //    object o = new object();
        //    System.Action paction = delegate()
        //    {
        //        int actidx;
        //        lock (o)
        //            {
        //            actidx = ipar++;
        //            }
        //        unsafe
        //            {
        //            fixed (byte* ptres = result, ptba = ba)
        //                {
        //                uint* pr = (uint*)ptres;
        //                uint* pa = (uint*)ptba;
        //                pr += actidx; pa += actidx;
        //                while (pr < ptres + len)
        //                    {
        //                    *pr = ~(*pa);
        //                    pr += paralleldegree; pa += paralleldegree;
        //                    }
        //                }
        //            }
        //    };
        //    System.Action[] actions = new Action[paralleldegree];
        //    for (int i = 0; i < paralleldegree; i++)
        //        {
        //        actions[i] = paction;
        //        }
        //    Parallel.Invoke(actions);
        //    return result;
        //    }
            
        //public static byte[] Bin_ShiftLeft(this byte[] ba, int bits)
        //    {
        //    int ipar = 0;
        //    object o = new object();

        //    int len = ba.Length;
        //    if (bits >= len * bitsinbyte) return new byte[len];
        //    int shiftbits = bits % bitsinuint;
        //    int shiftuints = bits / bitsinuint;
        //    byte[] result = new byte[len];

        //    if (len > 1)
        //        {
        //        // first uint is shifted without carry from previous byte (previous byte does not exist)
        //        unsafe
        //            {
        //            fixed (byte* fpba = ba, fpres = result)
        //                {
        //                uint* pres = (uint*)fpres + shiftuints;
        //                uint* pba = (uint*)fpba;
        //                *pres = *pba << shiftbits;
        //                }
        //            }
        //        System.Action paction = delegate()
        //            {
        //                int actidx;
        //                lock (o)
        //                    {
        //                    actidx = ipar++;
        //                    }
        //                unsafe
        //                    {
        //                    fixed (byte* fpba = ba, fpres = result)
        //                        {
        //                        // pointer to results; shift the bytes in the result
        //                        // (i.e. move left the pointer to the result)
        //                        uint* pres = (uint*)fpres + shiftuints + actidx + 1;
        //                        // pointer to original data, second byte
        //                        uint* pba1 = (uint*)fpba + actidx + 1;
        //                        if (shiftbits == 0)
        //                            {
        //                            while (pres < fpres + len)
        //                                {
        //                                *pres = *pba1;
        //                                pres += paralleldegree; pba1 += paralleldegree;
        //                                }
        //                            }
        //                        else
        //                            {
        //                            // pointer to original data, first byte
        //                            uint* pba2 = (uint*)fpba + actidx;
        //                            while (pres < fpres + len)
        //                                {
        //                                *pres = *pba2 >> (bitsinuint - shiftbits) | *pba1 << shiftbits;
        //                                pres += paralleldegree; pba1 += paralleldegree; pba2 += paralleldegree;
        //                                }
        //                            }
        //                        }
        //                    };

        //            };
        //        System.Action[] actions = new Action[paralleldegree];
        //        for (int i = 0; i < paralleldegree; i++)
        //            {
        //            actions[i] = paction;
        //            }
        //        Parallel.Invoke(actions);
        //        }

        //    return result;
        //    }
            
        //public static byte[] Bin_ShiftRight(this byte[] ba, int bits)
        //    {
        //    int ipar = 0;
        //    object o = new object();
        //    int len = ba.Length;
        //    if (bits >= len * bitsinbyte) return new byte[len];
        //    int ulen = len / uintsize + 1 - (uintsize - (len % uintsize)) / uintsize;
        //    int shiftbits = bits % bitsinuint;
        //    int shiftuints = bits / bitsinuint;
        //    byte[] result = new byte[len];

        //    if (len > 1)
        //        {
        //        unsafe
        //            {
        //            fixed (byte* fpba = ba, fpres = result)
        //                {
        //                uint* pres = (uint*)fpres + ulen - shiftuints - 1;
        //                uint* pba = (uint*)fpba + ulen - 1;
        //                *pres = *pba >> shiftbits;
        //                }
        //            }
        //        System.Action paction = delegate()
        //            {
        //                int actidx;
        //                lock (o)
        //                    {
        //                    actidx = ipar++;
        //                    }
        //                unsafe
        //                    {
        //                    fixed (byte* fpba = ba, fpres = result)
        //                        {
        //                        // pointer to results; shift the bytes in the result
        //                        // (i.e. move left the pointer to the result)
        //                        uint* pres = (uint*)fpres + actidx;
        //                        // pointer to original data, first useful byte
        //                        uint* pba1 = (uint*)fpba + shiftuints + actidx;
        //                        if (shiftbits == 0)
        //                            {
        //                            while (pres < ((uint*)fpres) + ulen - shiftuints - 1)
        //                                {
        //                                *pres = *pba1;
        //                                // increment pointers to next position
        //                                pres += paralleldegree; pba1 += paralleldegree;
        //                                }
        //                            }
        //                        else
        //                            {
        //                            // pointer to original data, second useful byte
        //                            uint* pba2 = (uint*)fpba + shiftuints + actidx + 1;
        //                            while (pres < ((uint*)fpres) + ulen - shiftuints - 1)
        //                                {
        //                                // Core shift operation
        //                                *pres = (*pba1 >> shiftbits | *pba2 << (bitsinuint - shiftbits));
        //                                // increment pointers to next position
        //                                pres += paralleldegree; pba1 += paralleldegree; pba2 += paralleldegree;
        //                                }
        //                            }
        //                        }
        //                    };
        //            };
        //        System.Action[] actions = new Action[paralleldegree];
        //        for (int i = 0; i < paralleldegree; i++)
        //            {
        //            actions[i] = paction;
        //            }
        //        Parallel.Invoke(actions);
        //        }
        //    return result;
        //    }
        }
}
