using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiNiJSON;
public class PayInfo
{
    private string oderId;       //订单号
    private string productName;   //商品名称
    private string productId; //商品Id
    private int price; //单价,以分为单位
    private int count; //一共买的数量
    private string notify_url;


    public PayInfo(string _oderId, string _productName, string _productId, int _unitPrice, int _count, string _notify_url)
    {
        oderId = _oderId;
        productName = _productName;
        productId = _productId;
        price = _unitPrice;
        count = _count;
        notify_url = _notify_url;
    }

    public string GetOrderId()
    {
        return oderId;
    }

    public string GetProductName()
    {
        return productName;
    }

    public string GetProductId()
    {
        return productId;
    }

    public int GetPrice()
    {
        return price;
    }

    public int GetCount()
    {
        return count;
    }

    public string GetNotify_Url()
    {
        return notify_url;
    }

    public string GetPayInfoStr()
    {
        Dictionary<string, string> requestDict = new Dictionary<string, string>();
        requestDict["order_id_cp"] = oderId;
        requestDict["account_id"] = ChannelUserInfo.Instance.AccountId;                  //游戏帐号Id
        requestDict["channel_id"] = ChannelUserInfo.Instance.ChannelId;                  //渠道Id
        requestDict["channel_userid"] = ChannelUserInfo.Instance.ChannelUserId;          //渠道UserId
        requestDict["role_id"] = ChannelUserInfo.Instance.RoleId;                        //角色Id
        requestDict["role_name"] = ChannelUserInfo.Instance.RoleName;                    //角色名
        requestDict["zone_id"] = ChannelUserInfo.Instance.ZoneId;                        //区服ID
        requestDict["zone_name"] = ChannelUserInfo.Instance.ZoneName;                    //区服名称
        requestDict["product_id"] = productId;                  //商品ID
        requestDict["product_name"] = productName;              //商品名称
        requestDict["price"] = price.ToString();            //商品单价

        requestDict["param1"] = "";
        requestDict["param2"] = "";
        requestDict["param3"] = "";

        //SDK支付成功后通知游戏服的回调地址
        requestDict["notify_url"] = notify_url;
        //用户token
        requestDict["token"] = "";
        //用户余额
        requestDict["user_balance"] = "";
        //用户vip等级
        requestDict["user_vip"] = "";
        //用户等级
        requestDict["user_lv"] = "";
        //用户帮派    
        requestDict["user_party"] = "";
        string strPayOrderInfo = Json.Serialize(requestDict);
        return strPayOrderInfo;
    }


    public string GetReYunPayStartStr()
    {
        //transactionId  交易的流水号
        //paymentType     支付类型     支付宝
        //currencyType    货币类型 CNY人民币、USD美金
        //currencyAmount  金额 元
        Dictionary<string, string> requestDict = new Dictionary<string, string>();
        requestDict["transactionId"] = oderId;
        requestDict["paymentType"] = "weixinpay";                   //支付类型     支付宝
        requestDict["currencyType"] = "CNY";                 // 货币类型 CNY人民币、USD美金
        requestDict["currencyAmount"] = (price * count /100).ToString();          // 金额 元
        requestDict["virtualCoinAmount"] = count.ToString();                       
        requestDict["iapName"] = productName;                    //
        requestDict["iapAmount"] ="1";                        
        string strPayOrderInfo = Json.Serialize(requestDict);
        return strPayOrderInfo;
    }
}
