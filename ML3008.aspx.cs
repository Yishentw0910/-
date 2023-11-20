/*
20130613 ADD BY ADAM Reason. 增加保險費的判斷邏輯
20131022 ADD BY SS ADAM Reason.AR附追索權無須列印保證人保證書
20150129 ADD By ChrisFu Reason. 增加 應收帳款管理同意書、支付價金申請書、讓與明細表 三支報表
20180625 ADD BY SS ADAM REASON.事務機增加交貨與驗收證明書
20231130 ADD 設備系統優化 增加文件 & 條件
*/
/*
ML3008R01 租賃契約書 營/資租事務機
ML3008R02 租賃契約書 營租非事務機
ML3008R03 租賃契約書 資租非事務機
ML3008R04 分期付款契約書 分期合約書
ML3008R05 附條件買賣契約書 分期附條買合約書
ML3008R06 國內應收帳款收買合約書 AR附追索權合約書
ML3008R07 國內應收帳款收買合約書 AR無附追索權合約書
ML3008R08 交貨與驗收證明書(營/資租)
ML3008R09 非AR保證人保證書
ML3008R10 AR保證人保證書
ML3008R11 訂購契約書(三方)
ML3008R12 訂購契約書(兩方)
ML3008R13 租賃物返還同意書
ML3008R14 擔保品提供證書
ML3008R15 動產抵押契約書
ML3008R16 應收帳款債權轉讓通知書
ML3008R17 保險切結書
ML3008R18 應收帳款管理同意書
ML3008R19 支付價金申請書
ML3008R20 讓與明細表
ML3008R21 買賣契約書
ML3008R22 本票
ML3008R23 指定付款同意書
ML3008R24 全額付款確認書(兩方版本)-確認客戶已取得產權
ML3008R25 全額付款確認書(三方版本)-客戶發票與實撥金額有落差
ML3008R26 全額付款確認書(三方版本)-客戶訂金抵頭款
ML3008R27 車輛動產抵押契約書
ML3008R28 交貨與驗收證明書(分期)
ML3008R29 切  結  書
ML3008R30 動產擔保交易(附條件買賣) 登記申請書
ML3008R31 動產擔保交易登記標的物明細表
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ML30_ML3008 : System.Web.UI.Page
{
	
    protected void Page_Init(object sender, EventArgs e)
    {
        this.txtCASENO.Attributes.Add("onkeyup", "toUpper(\"" + this.txtCASENO.ClientID.ToString() + "\")");
        this.txtCNTRNO.Attributes.Add("onkeyup", "toUpper(\"" + this.txtCNTRNO.ClientID.ToString() + "\")");
        //this.ddlSource.Attributes.Add("onchange", "setCASENO('" + this.ddlSource.ClientID.ToString() + "', '" + this.txtCNTRNO.ClientID.ToString() + "')");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
		
        if (!IsPostBack)
        {
            if (Session["UserID"] == null) { Response.Redirect("/LC/LOGIN.ASP"); return; }
            //if (Session["UserID"] == null) { createSession(); }
            SESSION_SET();
            cmdCLEAR_Click(null, null);
        }

    }

    protected void cmdQUERY_Click(object sender, EventArgs e)
    {
		
        this.ddlDocuments.Items.Clear();
		RTYPE.Value = "";
        DataSet ds = new DataSet();
        string MSG = "";
        SSNET3.SSNETSP3 ss = new SSNET3.SSNETSP3();
        try
        {
            string[] CltAryLogin = new string[2];
            CltAryLogin[0] = SessSQLSVRNM.Value.ToString().Trim();
            CltAryLogin[1] = SessDBNM.Value.ToString().Trim();
            //CltAryLogin[0] = "SSSQYHFC01_R2";
            //CltAryLogin[1] = "ML";
            
            object[] LParm = new object[4];

            if (this.ddlSource.SelectedValue == "01" )
            {
                LParm[0] = txtCASENO.Text.Trim();
				LParm[1] = SessUSERID.Value;
				LParm[2] = SessDEPTID.Value;
				
                ds = ss.SPRetB(CltAryLogin, "SP_ML3008_Q01", LParm, ref MSG);
				
				// 20130611 ADD BY ADAM Reason.增加邏輯判斷
				if (ds.Tables.Count == 0)
				{
					ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('" + MSG + "');", true);
					return;
				}
				
				if (ds.Tables[0].Rows.Count == 0)
				{
					ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('案件尚無資料');", true);
					return;
				}
            }
            else
            {
				/*
                if (txtCNTRNO.Text != "")
                {
                    LParm[0] = txtCNTRNO.Text.Trim();
                    ds = ss.SPRetB(CltAryLogin, "SP_ML3008_Q02", LParm, ref MSG);
                }
                else
                {
                    LParm[0] = txtCASENO.Text.Trim();
                    ds = ss.SPRetB(CltAryLogin, "SP_ML3008_Q01", LParm, ref MSG);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('此案件為多筆動用，請輸入合約編號!');", true);
                        return;
                    }
                }
				*/
				
				//20130611 ADD BY ADAM Reason.修正列印來源是合約的邏輯
				LParm[0] = txtCASENO.Text.Trim();
				LParm[1] = txtCNTRNO.Text.Trim();
				LParm[2] = SessUSERID.Value;
				LParm[3] = SessDEPTID.Value;
				ds = ss.SPRetB(CltAryLogin, "SP_ML3008_Q02", LParm, ref MSG);
				if (ds.Tables.Count == 0)
				{
					ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('" + MSG + "');", true);
					return;
				}
				
				if (ds.Tables[0].Rows.Count == 0)
				{
					ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('合約尚無資料');", true);
					return;
				}
				if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
				{
					ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('此案件為多筆動用，請輸入合約編號!');", true);
					return;
				}
				
            }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.hfCASEID.Value = ds.Tables[0].Rows[0]["CASEID"].ToString();
                string mainType = ds.Tables[0].Rows[0]["MAINTYPE"].ToString();
                string subType = ds.Tables[0].Rows[0]["SUBTYPE"].ToString();
                string transType = ds.Tables[0].Rows[0]["TRANSTYPE"].ToString();
				//20130613 ADD BY ADAM Reason. 增加保險費的判斷邏輯
				int INSURANCE = int.Parse(ds.Tables[0].Rows[0]["INSURANCE"].ToString());
                //20231130 ADD 設備系統優化 增加條件 & 文件
                string EXPIREPROC = ds.Tables[0].Rows[0]["EXPIREPROC"].ToString();  //期滿處理方式 01:供應商買回 02:客戶第三方買回 03:其他 04:供應商買回(批次約定)
                string RESIDUALS = ds.Tables[0].Rows[0]["RESIDUALS"].ToString();  //殘值
                string PAYMENT = ds.Tables[0].Rows[0]["PAYMENT"].ToString();  //有無頭期款或保證金
                string GUARANTEEBILL = ds.Tables[0].Rows[0]["GUARANTEEBILL"].ToString();  //1:有保證人且「簽訂大本票為是」
                string PRODCD = ds.Tables[0].Rows[0]["PRODCD"].ToString();  //案件產品別
                //20130626 ADD BY ADAM Reason. 增加全部選項
                this.ddlDocuments.Items.Add(new ListItem("全部","ML3008R00"));

                //UPD BY VICKY 20131126 把MAINTYPE入隱藏欄位
                hdMAINTYPE.Value = ds.Tables[0].Rows[0]["MAINTYPE"].ToString();
                this.panPRINT.Enabled = false;

                switch (mainType)
                {
                    case "01":
						
                        if (subType == "01")   //營租事務機
                        {
                            this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R01"));
							RTYPE.Value+="01,";
                            //20200213 ADD BY SS ADAM REASON.事務機增加交貨與驗收證明書
                            this.ddlDocuments.Items.Add(new ListItem("交貨與驗收證明書", "ML3008R08"));
                            RTYPE.Value += "08,";

							//20130613 ADD BY ADAM Reason. 增加保險費的判斷邏輯
							if (INSURANCE == 0)
							{
								this.ddlDocuments.Items.Add(new ListItem("保險切結書", "ML3008R17"));
								RTYPE.Value+="17,";
							}
                        }
                        else
                        {
                            this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R02"));
                            this.ddlDocuments.Items.Add(new ListItem("交貨與驗收證明書", "ML3008R08"));
                            //20231130 3.	取消「保證人保證書」，條文併入主約。
                            //this.ddlDocuments.Items.Add(new ListItem("保證人保證書", "ML3008R09"));
                            //RTYPE.Value+="02,08,09,";
                            RTYPE.Value+="02,08,";
                            if (transType == "02") //三方
							{
                                this.ddlDocuments.Items.Add(new ListItem("設備訂購契約書", "ML3008R11"));
								RTYPE.Value+="11,";
                                //20231130 ADD 全額付款確認書
                                if (PAYMENT=="1")   //「有頭期款或保證金」且為三方案件→三方版本(客戶訂金抵頭款)
                                {
                                    this.ddlDocuments.Items.Add(new ListItem("全額付款確認書", "ML3008R26"));
                                    RTYPE.Value += "26,";
                                }
                            }
                            else//兩方
							{
                                this.ddlDocuments.Items.Add(new ListItem("設備訂購契約書", "ML3008R12"));
								RTYPE.Value+="12,";
                                //20231130 ADD 全額付款確認書
                                if (PAYMENT == "1")   //「有頭期款或保證金」且為兩方案件→兩方版本(客戶發票與實撥金額有落差)
                                {
                                    this.ddlDocuments.Items.Add(new ListItem("全額付款確認書", "ML3008R25"));
                                    RTYPE.Value += "25,";
                                }
                                else if (PAYMENT == "2")    //「無頭期款或保證金」且為兩方案件→三方版本(確認客戶已取得產權)。
                                {
                                    this.ddlDocuments.Items.Add(new ListItem("全額付款確認書", "ML3008R24"));
                                    RTYPE.Value += "24,";
                                }
                            }
                            this.ddlDocuments.Items.Add(new ListItem("租賃物返還同意書", "ML3008R13"));
							RTYPE.Value+="13,";
							//20130613 ADD BY ADAM Reason. 增加保險費的判斷邏輯
							if (INSURANCE == 0)
							{
								this.ddlDocuments.Items.Add(new ListItem("保險切結書", "ML3008R17"));
								RTYPE.Value+="17,";
							}
                        }
                        //20231130 ADD 期滿處理方式
                        if (EXPIREPROC == "01" || EXPIREPROC == "02") //期滿處理方式為「供應商買回」或「客戶第三方買回」
                        {
                            this.ddlDocuments.Items.Add(new ListItem("期滿買賣契約書", "ML3008R21"));
                            RTYPE.Value += "21,";
                        }
                        //20231130 ADD 大本票
                        if (GUARANTEEBILL == "1") //有保證人且「簽訂大本票為是」
                        {
                            this.ddlDocuments.Items.Add(new ListItem("期滿買賣契約書", "ML3008R22"));
                            RTYPE.Value += "22,";
                        }
                        //20231130 ADD 指定付款同意書
                        this.ddlDocuments.Items.Add(new ListItem("指定付款同意書", "ML3008R23"));
                        RTYPE.Value += "23,";
                        break;
                    case "02":
                        if (subType == "01")   //資租事務機
                        {
                            this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R01"));
							RTYPE.Value+="01,";
                            //20200213 ADD BY SS ADAM REASON.事務機增加交貨與驗收證明書
                            this.ddlDocuments.Items.Add(new ListItem("交貨與驗收證明書", "ML3008R08"));
                            RTYPE.Value += "08,";

							//20130613 ADD BY ADAM Reason. 增加保險費的判斷邏輯
							if (INSURANCE == 0)
							{
								this.ddlDocuments.Items.Add(new ListItem("保險切結書", "ML3008R17"));
								RTYPE.Value+="17,";
							}
                        }
                        else
                        {
                            this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R03"));
                            this.ddlDocuments.Items.Add(new ListItem("交貨與驗收證明書", "ML3008R08"));
                            //20231130 取消「保證人保證書」，條文併入主約
                            //this.ddlDocuments.Items.Add(new ListItem("保證人保證書", "ML3008R09"));
                            //RTYPE.Value+="02,08,09,";
                            RTYPE.Value += "02,08,";
                            if (transType == "02") //三方
							{
                                this.ddlDocuments.Items.Add(new ListItem("設備訂購契約書", "ML3008R11"));
								RTYPE.Value+="11,";
                                //20231130 ADD 全額付款確認書
                                if (PAYMENT == "1")   //「有頭期款或保證金」且為三方案件→三方版本(客戶訂金抵頭款)
                                {
                                    this.ddlDocuments.Items.Add(new ListItem("全額付款確認書", "ML3008R26"));
                                    RTYPE.Value += "26,";
                                }
                            }
                            else//兩方
							{
                                this.ddlDocuments.Items.Add(new ListItem("設備訂購契約書", "ML3008R12"));
								RTYPE.Value+="12,";
							}
                            this.ddlDocuments.Items.Add(new ListItem("租賃物返還同意書", "ML3008R13"));
							RTYPE.Value+="13,";
							//20130613 ADD BY ADAM Reason. 增加保險費的判斷邏輯
							if (INSURANCE == 0)
							{
								this.ddlDocuments.Items.Add(new ListItem("保險切結書", "ML3008R17"));
								RTYPE.Value+="17,";
							}
                            //20231130 ADD 全額付款確認書
                            if (PAYMENT == "1")   //「有頭期款或保證金」且為兩方案件→兩方版本(客戶發票與實撥金額有落差)
                            {
                                this.ddlDocuments.Items.Add(new ListItem("全額付款確認書", "ML3008R25"));
                                RTYPE.Value += "25,";
                            }
                            else if (PAYMENT == "2")    //「無頭期款或保證金」且為兩方案件→三方版本(確認客戶已取得產權)。
                            {
                                this.ddlDocuments.Items.Add(new ListItem("全額付款確認書", "ML3008R24"));
                                RTYPE.Value += "24,";
                            }
                        }
                        //20231130 ADD 殘值
                        if (RESIDUALS != "0") //殘值不為0
                        {
                            this.ddlDocuments.Items.Add(new ListItem("期滿買賣契約書", "ML3008R21"));
                            RTYPE.Value += "21,";
                        }
                        //20231130 ADD 大本票
                        if (GUARANTEEBILL == "1") //有保證人且「簽訂大本票為是」
                        {
                            this.ddlDocuments.Items.Add(new ListItem("期滿買賣契約書", "ML3008R22"));
                            RTYPE.Value += "22,";
                        }
                        //20231130 ADD 指定付款同意書
                        this.ddlDocuments.Items.Add(new ListItem("指定付款同意書", "ML3008R23"));
                        RTYPE.Value += "23,"; break;
                    case "03":
                        if (subType == "03")   //分期設備動保
                        {
                            this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R04"));
                            this.ddlDocuments.Items.Add(new ListItem("交貨與驗收證明書", "ML3008R28"));
                            //20231130 取消「保證人保證書」，條文併入主約
                            //this.ddlDocuments.Items.Add(new ListItem("保證人保證書", "ML3008R09"));
                            //RTYPE.Value+="04,08,09,";
                            RTYPE.Value+="04,08,";
                            if (transType == "02") //三方
							{
                                this.ddlDocuments.Items.Add(new ListItem("設備訂購契約書", "ML3008R11"));
								RTYPE.Value+="11,";
							}
                            else//兩方
							{
                                this.ddlDocuments.Items.Add(new ListItem("設備訂購契約書", "ML3008R12"));
								RTYPE.Value+="12,";
							}
                            this.ddlDocuments.Items.Add(new ListItem("擔保品提供證書", "ML3008R14"));
                            this.ddlDocuments.Items.Add(new ListItem("動產抵押契約書", "ML3008R15"));
							RTYPE.Value+="14,15,";
							//20130613 ADD BY ADAM Reason. 增加保險費的判斷邏輯
                            //20231130 改分期時不顯示
							//if (INSURANCE == 0)
							//{
							//	this.ddlDocuments.Items.Add(new ListItem("保險切結書", "ML3008R17"));
							//	RTYPE.Value+="17,";
							//}
                            //20231130 ADD 案件產品別
                            if (PRODCD=="03")//重車
                            {
                                this.ddlDocuments.Items.Add(new ListItem("車輛動產抵押契約書", "ML3008R27"));
                                RTYPE.Value += "27,";
                            }
                            else
                            {
                                this.ddlDocuments.Items.Add(new ListItem("動產擔保交易登記標的物明細表", "ML3008R31"));
                                RTYPE.Value += "31,";
                                this.ddlDocuments.Items.Add(new ListItem("切結書", "ML3008R29"));
                                RTYPE.Value += "29,";
                                this.ddlDocuments.Items.Add(new ListItem("動產擔保交易登記申請書", "ML3008R30"));
                                RTYPE.Value += "30,";
                            }
                        }
                        else
                        {
                            switch (subType)
                            {
                                case "01":   //分期原物料
                                    this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R04"));
									RTYPE.Value+="04,";
                                    break;
                                case "02":   //分期附條買
                                    this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R05"));
									RTYPE.Value+="05,";
                                    if (PRODCD == "03")//重車
                                    {
                                        this.ddlDocuments.Items.Add(new ListItem("車輛動產抵押契約書", "ML3008R27"));
                                        RTYPE.Value += "27,";
                                    }
                                    else
                                    {
                                        this.ddlDocuments.Items.Add(new ListItem("動產擔保交易登記申請書", "ML3008R30"));
                                        RTYPE.Value += "30,";
                                        this.ddlDocuments.Items.Add(new ListItem("切結書", "ML3008R29"));
                                        RTYPE.Value += "29,";
                                        this.ddlDocuments.Items.Add(new ListItem("動產擔保交易登記標的物明細表", "ML3008R31"));
                                        RTYPE.Value += "31,";

                                    }
                                    break;
                                //20131107 ADD BY SS ADAM Reason.增加設備無設定的合約書
                                case "04":  //設備無設定
                                    this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R04"));
                                    RTYPE.Value+="04,";
                                    break;
                            }
                            this.ddlDocuments.Items.Add(new ListItem("交貨與驗收證明書", "ML3008R08"));
                            //20231130 取消「保證人保證書」，條文併入主約
                            //this.ddlDocuments.Items.Add(new ListItem("保證人保證書部", "ML3008R09"));
                            //RTYPE.Value+="08,09,";
                            RTYPE.Value+="08,";
                            if (transType == "02") //三方
							{
                                this.ddlDocuments.Items.Add(new ListItem("設備訂購契約書", "ML3008R11"));
								RTYPE.Value+="11,";
							}
                            else//兩方
							{
                                this.ddlDocuments.Items.Add(new ListItem("設備訂購契約書", "ML3008R12"));
								RTYPE.Value+="12,";
							}
							//20130613 ADD BY ADAM Reason. 增加保險費的判斷邏輯
                            //20231130 改分期時不顯示
							//if (INSURANCE == 0)
							//{
							//	this.ddlDocuments.Items.Add(new ListItem("保險切結書", "ML3008R17"));
							//	RTYPE.Value+="17,";
							//}
                        }
                        //20231130 ADD 大本票
                        if (GUARANTEEBILL == "1") //有保證人且「簽訂大本票為是」
                        {
                            this.ddlDocuments.Items.Add(new ListItem("期滿買賣契約書", "ML3008R22"));
                            RTYPE.Value += "22,";
                        }
                        //20231130 ADD 指定付款同意書
                        this.ddlDocuments.Items.Add(new ListItem("指定付款同意書", "ML3008R23"));
                        RTYPE.Value += "23,"; this.panPRINT.Enabled = true;
                        break;
                    case "04":
                        if (ds.Tables[0].Rows[0]["RECOURSE"].ToString() == "Y")    //AR附追索權
						{
                            this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R06"));
							//RTYPE.Value+="06,";
                            //20131022 ADD BY SS ADAM Reason.AR附追索權無須列印保證人保證書
                            //RTYPE.Value+="06,10,";
                            RTYPE.Value+="06,";
                            //20231130 取消「保證人保證書」，條文併入主約
                            //this.ddlDocuments.Items.Add(new ListItem("保證人保證書", "ML3008R10"));
                        }
                        else
						{
                            this.ddlDocuments.Items.Add(new ListItem("合約書", "ML3008R07"));
							RTYPE.Value+="07,";
						}
						//this.ddlDocuments.Items.Add(new ListItem("保證人保證書", "ML3008R10"));
						this.ddlDocuments.Items.Add(new ListItem("國內應收帳款債權轉讓通知書", "ML3008R16"));
                        //20131022 ADD BY SS ADAM Reason.AR附追索權無須列印保證人保證書
                        //RTYPE.Value+="10,16,";
						//RTYPE.Value+="16,";
                        //20150129 ADD By ChrisFu Reason. 增加 應收帳款管理同意書、支付價金申請書、讓與明細表 三支報表
                        this.ddlDocuments.Items.Add(new ListItem("應收帳款管理同意書", "ML3008R18"));
                        this.ddlDocuments.Items.Add(new ListItem("支付價金申請書", "ML3008R19"));
                        this.ddlDocuments.Items.Add(new ListItem("讓與明細表", "ML3008R20"));
                        RTYPE.Value += "16,18,19,20,";
                        break;
                }
                this.ddlSource.Enabled = false;
                this.txtCASENO.Enabled = false;
                this.txtCNTRNO.Enabled = false;
                this.cmdPrint.Enabled = true;
            }
            else
            {
                if(ddlSource.SelectedValue=="02")
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('合約尚無資料!');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('案件尚無資料!');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('查詢錯誤，請連絡資訊人員!'"+ ex.Message +");", true);
        }
		
        ds.Dispose();
        ss.Dispose();
    }

    protected void cmdCLEAR_Click(object sender, EventArgs e)
    {
        this.ddlSource.Enabled = true;
        this.txtCASENO.Enabled = true;
        //this.txtCNTRNO.Enabled = false;
        this.txtCNTRNO.Enabled = true;

        this.ddlSource.SelectedIndex = 0;
        this.txtCNTRNO.Text = "";
        this.txtCASENO.Text = "";
        this.ddlDocuments.Items.Clear();
        this.cmdPrint.Enabled = false;
        this.panPRINT.Enabled = false;  //UPD BY VICKY 20131126
        //20140715 ADD BY SS ADAM REASON.增加動產設定單位
        this.optIMVSETUP1.Checked = false;
        this.optIMVSETUP2.Checked = true;
    }

    protected void cmdPrint_Click(object sender, EventArgs e)
    {
		
        SSNET3.SSNETSP2 ss = new SSNET3.SSNETSP2();
        string msg = "";
        string[] CltAryLogin = new string[2];
        CltAryLogin[0] = SessSQLSVRNM.Value.ToString().Trim();
        CltAryLogin[1] = SessDBNM.Value.ToString().Trim();

        object[] LParm = new object[2];
        if (txtCNTRNO.Text != "")
            LParm[0] = txtCNTRNO.Text.Trim();
        else
            LParm[0] = hfCASEID.Value.Trim();
        LParm[1] = SessUSERID.Value;
        if (ss.ExeSP(CltAryLogin, "SP_ML3008R01_LOG", LParm, ref msg))
        {

            //UPD BY VICKY 20131126 新增期付款內容是否顯示判斷
            string showMONEY = "", showDAY = "";
            if (optMONEY_Y.Checked) { showMONEY = "Y"; } else { showMONEY = "N"; }
            if (optPAYDATE_Y.Checked) { showDAY = "Y"; } else { showDAY = "N"; }

            //20140715 ADD BY SS ADAM REASON.增加動產設定單位
            string showIMVSETUP = "";
            if (optIMVSETUP2.Checked) { showIMVSETUP = "01"; } else { showIMVSETUP = "02"; }
            //string url = "window.open('" + ddlDocuments.SelectedValue.Trim() + ".aspx?SOURCE=" + ddlSource.SelectedValue.Trim() + 
            //    "&CASEID=" + hfCASEID.Value.ToString().Trim() + "&CNTRNO=" + txtCNTRNO.Text.Trim() + "&RPTIDX=" + RTYPE.Value + "');";

            string url = "window.open('" + ddlDocuments.SelectedValue.Trim() + ".aspx?SOURCE=" + ddlSource.SelectedValue.Trim() +
                    "&CASEID=" + hfCASEID.Value.ToString().Trim() + "&CNTRNO=" + txtCNTRNO.Text.Trim() + "&RPTIDX=" + RTYPE.Value + "&showMONEY=" + showMONEY + "&showDAY=" + showDAY 
                    //20140715 ADD BY SS ADAM REASON.增加動產設定單位
                    + "&showIMVSETUP=" + showIMVSETUP + "');";
            
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "PrintRPT", url, true);
        }else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "ERR1", "alert('個資LOG新增失敗，請連絡資訊人員!');", true);
        ss.Dispose();
    }

    public void SESSION_SET()
    {
        SessUSERID.Value = Session["UserID"].ToString().Trim();
        SessUSERNM.Value = Session["USERNM"].ToString().Trim();
        SessEMPLID.Value = Session["EMPLID"].ToString().Trim();
        SessBRNHCD.Value = Session["BRNHCD"].ToString().Trim();
        SessDBNM.Value = Session["DBNM"].ToString().Trim();
        //SessMTSSVRNM.Value = Session["MTSSVRNM"].ToString().Trim();
        SessSQLSVRNM.Value = Session["SQLSVRNM"].ToString().Trim();
        SessSYSCD.Value = Session["SYSCD"].ToString().Trim();
        //SessGROUPID.Value = Session["GROUPID"].ToString().Trim();
        //SessDATAGP.Value = Session["DATAGP"].ToString().Trim();
        SessDLRCD.Value = Session["DLRCD"].ToString().Trim();
        SessDLRNM.Value = Session["DLRNM"].ToString().Trim();
        SessDEPTID.Value = Session["DEPTID"].ToString().Trim();
        SessDEPTNM.Value = Session["DEPTNM"].ToString().Trim();
    }

    private void createSession()
    {
        Session.Add("UserID", "ROOT");
        Session.Add("USERNM", "ROOT");
        Session.Add("LOGINTIME", DateTime.Now.ToString().Trim());
        Session.Add("EMPLID", "ROOT");
        Session.Add("BRNHCD", "AC00");

        Session.Add("DBNM", "LE");
        //Session.Add("MTSSVRNM", "WIN2KVPCSS");
        Session.Add("SQLSVRNM", "SSSQYHFC01_R2");
        Session.Add("SYSCD", "LE");
        //Session.Add("GROUPID", "CR01");
        //Session.Add("DATAGP", "B");
        Session.Add("DLRCD", "01");
        Session.Add("DLRNM", "和運租車");
        //Session.Add("DEPTID", "AC00");
        Session.Add("DEPTNM", "客服部");

    }

    protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSource.SelectedValue == "02")
            txtCNTRNO.Enabled = true;
        else
            txtCNTRNO.Enabled = false;
    }

    //UPD BY VICKY 20131126 期付款資料顯示勾選
   
    protected void ddlDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (hdMAINTYPE.Value == "03")
        {
            string strSELECT = ddlDocuments.SelectedValue.ToString().Trim();
            optMONEY_Y.Checked = true;
            optPAYDATE_Y.Checked = true;

            if (strSELECT == "ML3008R00" || strSELECT == "ML3008R04" || strSELECT == "ML3008R05")
            {

                panPRINT.Enabled = true;
            }
            else
            {
                panPRINT.Enabled = false;
            }
        }

    }
}