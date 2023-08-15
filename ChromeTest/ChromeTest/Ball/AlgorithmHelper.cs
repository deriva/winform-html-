
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChromeTest.Ball
{
    /// <summary>
    /// 算法帮助类
    /// </summary>
    public class AlgorithmHelper
    {
        private static  int qishu = 100;
        /// <summary>
        /// 计算离散差值
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static double CalcLiSanChaZhi(int[] lst)
        {
            //计算总和
            var sum = lst.Sum();
            //1计算平均值（均值）：
            var avg = sum * 1.0d / lst.Length;
            var dd = 0.0d;
            lst.ToList().ForEach(x =>
            {
                //2计算每个数据点与平均值之差的平方：
                //3.计算平方差值的总和：
                dd += Math.Sqrt(Math.Abs(x * 1.0d - avg))*100000- 140000;
            });
            //4.计算离散方差：
            return dd / lst.Length;
        }


        //var ss = "";$(".redqiu4 ").each(function (i, x) {
        //    if (i % 3 == 0) ss += $(x).text() + ",";
        ///中彩网获取
        //});console.log(ss)
        /// <summary>
        /// 计算一批的离散值 :12,12,3,4,5,,66,7,,
        /// </summary>
        /// <returns></returns>
        public static string CalcLisahn2(string tt)
        {

            var ss = new List<double>();
            var tt2 = "";
            var qq = qishu;
            var ttlst = tt.Split(',').Select(x => int.Parse(x)).ToList();
            for (var i = qq; i <= ttlst.Count; i++)
            {
                if (i == ttlst.Count() )
                {
                    var sssss = 0;
                }
                var s1 = ttlst.Skip(i - qq).Take(qq).ToList();
                var r = CalcLiSanChaZhi(s1.ToArray());
                ss.Add(r);
                tt2 += $"[{i - qq},{r}],";

            }
            var rr = string.Join(",", ss);

            return tt2.TrimEnd(',');


        }
        /// <summary>
        /// 反推下个数
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public static string FanTuiNext( string tt)
        {
            var dic = new Dictionary<int, double>();
            for (var i = 0; i < 10; i++) dic.Add(i, 0.0d);

            var dic2 = new Dictionary<int, double>();
            var ss = new List<double>();
            var ttlst2 = tt.Split(',');
          var  ttlst= ttlst2.Skip(ttlst2.Count()-qishu+1).Select(x => int.Parse(x)).ToList();
     
            var tt2 = "";
            foreach (var it in dic)
            {
                var arr = ttlst;
                arr.Add(it.Key);
                dic2[it.Key] = CalcLiSanChaZhi(arr.ToArray());
                tt2 += $"[{it.Key},{dic2[it.Key] }],\r\n";
            } 
            return tt2.TrimEnd(',');
        }


        public static  string GetFc()
        {
            var bj = "4,1,8,8,4,0,0,0,1,1,9,7,3,4,7,1,4,4,0,3,3,7,4,2,5,3,3,2,5,3,7,0,0,7,2,4,9,1,1,8,8,7,4,9,7,5,7,8,6,9,3,6,6,0,1,8,2,0,1,8,5,1,0,5,3,2,0,2,6,1,1,7,5,2,2,0,0,7,8,3,7,8,7,2,2,6,1,2,8,4,8,3,0,3,2,4,5,2,0,3,3,2,4,5,9,7,4,2,0,4,0,9,1,4,7,3,9,2,8,0,7,2,0,6,7,1,2,7,4,9,1,3,0,3,6,3,1,4,7,9,0,6,9,6,6,7,0,7,3,6,2,5,0,6,8,5,5,9,7,9,3,3,3,8,1,4,6,5,7,7,0,4,0,4,5,8,3,9,5,0,4,1,0,7,5,2,3,0,4,9,7,0,0,5,9,3,2,9,5,7,8,5,5,4,7,6,2,5,6,5,7,9,6,5,2,5,4,3,4,6,1,2,8,5,9,4,6,0,0,6,8,5,0,5,9,1,8,3,5,5,0,4,2,4,9,7,4,4,0,7,1,8,5,8,5,8,3,0,0,3,3,7,4,8,2,7,4,3,3,5,0,1,2,2,1,1,9,7,0,2,7,0,8,2,0,7,7,4,6,7,1,8,9,4,7,7,2,0,6,3";//下个是

            //var ttlst2 = bj.Split(',');
            //var ttlst = ttlst2.Skip(ttlst2.Count() - qishu).Take(qishu).Select(x => int.Parse(x)).ToList();
            //bj = string.Join(",", ttlst);
            var ss=   FanTuiNext( bj);
            var jo = new JObject();
            jo["data"]=  AlgorithmHelper.CalcLisahn2(bj);
            jo["nextdata"] = ss;
            return jo.ToString();
            //var tt = bj.Split(',').ToList();
            //tt= tt.Skip(tt.Count - 250).ToList();
            //AlgorithmHelper.FanTuiNext(0, string.Join(",", tt));
        }
    }

}