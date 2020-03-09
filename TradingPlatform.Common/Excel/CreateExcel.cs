//using OfficeOpenXml;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingPlatform.Model;

namespace TradingPlatform.Common.Excel
{
    public class CreateExcel
    {
        //public static StringBuilder Create(List<Buy_Order> orderList) {
        //    var sbHtml = new StringBuilder();
        //    sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
        //    sbHtml.Append("<tr>");
        //    var lstTitle = new List<string> { "编号","购买金额USDT", "付款金额RMB", "手续费RMB", "实际到账USDT", "付款人", "付款方式", "外部订单号","订单生成时间", "状态" };
        //    foreach (var item in lstTitle)
        //    {
        //        sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
        //    }
        //    sbHtml.Append("</tr>");

        //    foreach(var order in orderList)
        //    {
        //        sbHtml.Append("<tr>");
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", order.Id);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", order.RechargeAmount);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", order.Number);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", order.ServiceCharge);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", order.ArrivalAmount);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", order.BuyerName);
        //        var payType = CheckType(order.PayTypeID);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", payType);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", order.OutOrderNumber);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", order.CreateTime);
        //        var payStatu = CheckStatu(order.PayTypeID);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", payStatu);
        //        sbHtml.Append("</tr>");
        //    }
        //    sbHtml.Append("</table>");
        //    return sbHtml;
        //    ////第一种:使用FileContentResult
        //    //byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
        //    //return File(fileContents, "application/ms-excel", "fileContents.xls");

        //    //第二种:使用FileStreamResult
        //    //var fileStream = new MemoryStream(fileContents);
        //    //return File(fileStream, "application/ms-excel", "fileStream.xls");

        //    //第三种:使用FilePathResult
        //    //服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.
        //    //var fileName = Server.MapPath("~/Files/fileName.xls");
        //    //return File(fileName, "application/ms-excel", "fileName.xls");
        //}

        public static string CheckType(int num) {
            var type = "";
            switch (num)
            {
                case 1:
                    type="银联卡";
                    break;
                case 2:
                    type = "支付宝";
                    break;
                default:
                    break;
            }
            return type;
        }

        public static string CheckStatu(int num)
        {
            var statu = "";
            switch (num)
            {
                case 0:
                    statu="未付款";
                    break;
                case 1:
                    statu = "用户已付款";
                    break;
                case 2:
                    statu = "支付成功";
                    break;
                case 3:
                    statu = "确认到账";
                    break;
                case 4:
                    statu = "超时关闭";
                    break;
                default:
                    break;
            }
            return statu;
        }
    }
}