using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;

public partial class ABAWeb_ABA_Screens_AgentError : System.Web.UI.Page
{
   public DataTable objDTAgents;
   public DataRow objDRAgent;
   public DataTable objDTErrors;
   public DataRow objDRError;

   protected void Page_Init(object sender, EventArgs e)
   {
      if (!Page.IsPostBack)
      {
         makeTables();
      }
   }

   protected void Page_Load(object sender, EventArgs e)
   {
      this.lblErrMsg.Text = "";

      if (!Page.IsPostBack)
      {
         clear();
         this.txtAccountNbr.Focus();
         this.drpAgents.DataSource = getAgents(" = 0");
         this.drpAgents.DataBind();
      }
   }

   protected void txtAccountNbr_TextChanged(object sender, EventArgs e)
   {
      string queryString = "SELECT contactname FROM aba.contacts WHERE accountnumber = :accountnumber AND accountholder = 'Y'";
      OracleConnection myConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ABAProgrammerConnection"].ToString());
      OracleCommand myCommand = new OracleCommand(queryString, myConnection);
      myCommand.BindByName = true;
      OracleDataReader myReader = default(OracleDataReader);

      try
      {
         myConnection.Open();

         myCommand.Parameters.Clear();
         myCommand.Parameters.Add("accountnumber", this.txtAccountNbr.Text);

         myReader = myCommand.ExecuteReader();
         myReader.Read();
         if (myReader.HasRows)
         {
            this.lblAccountName.Text = myReader["contactname"].ToString();
         }
         else
         {
            this.lblAccountName.Text = "";
         }
         myReader.Close();
      }
      catch (Exception ex)
      {
         this.lblErrMsg.Text += "ERROR looking up the account record<br />";
         try
         {
            Master.HandleError(ex, myCommand, "look up the account record: ");
         }
         catch
         {
            this.lblErrMsg.Text += "Unable to complete request. Please contact your supervisor.<br />";
            this.lblErrMsg.Text += ex.Message + "<br />";
         }
      }
      finally
      {
         if (myReader != null && !myReader.IsClosed)
         {
            myReader.Close();
         }

         try
         {
            myCommand.Dispose();
         }
         catch (Exception)
         {
         }

         myConnection.Close();
         myConnection.Dispose();
      }
   }

   protected void drpStudents_SelectedIndexChanged(object sender, EventArgs e)
   {
      if (this.drpStudents.SelectedItem.Text == "Other")
      {
         this.txtOtherApp.Visible = true;
      }
      else
      {
         this.txtOtherApp.Visible = false;
      }
   }

   protected void txtOtherApp_TextChanged(object sender, EventArgs e)
   {
      string queryString1 = "SELECT ap_apref FROM aba.appld WHERE ap_apref = :ap_apref";
      OracleConnection myConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ABAProgrammerConnection"].ToString());
      OracleCommand myCommand1 = new OracleCommand(queryString1, myConnection);
      myCommand1.BindByName = true;
      OracleDataReader myReader1 = default(OracleDataReader);

      string queryString2 = "SELECT ap_apref FROM aba.appld WHERE fd_host_no = :fd_host_no AND ap_apref = :ap_apref";
      OracleCommand myCommand2 = new OracleCommand(queryString2, myConnection);
      myCommand2.BindByName = true;
      OracleDataReader myReader2 = default(OracleDataReader);

      try
      {
         myConnection.Open();

         myCommand1.Parameters.Clear();
         myCommand1.Parameters.Add("ap_apref", this.txtOtherApp.Text);

         myReader1 = myCommand1.ExecuteReader();
         myReader1.Read();
         if (!myReader1.HasRows)
         {
            this.lblErrMsg.Text += "Please enter a valid application number.<br />";
         }
         else
         {
            myCommand2.Parameters.Clear();
            myCommand2.Parameters.Add("fd_host_no", this.txtAccountNbr.Text);
            myCommand2.Parameters.Add("ap_apref", this.txtOtherApp.Text);

            myReader2 = myCommand2.ExecuteReader();
            myReader2.Read();
            if (!myReader2.HasRows)
            {
               this.lblErrMsg.Text += "The application number does not match the account.<br />";
            }
            myReader2.Close();
         }
         myReader1.Close();
      }
      catch (Exception ex)
      {
         this.lblErrMsg.Text += "ERROR looking up the application record<br />";

         try
         {
            Master.HandleError(ex, myCommand1, "look up application record: ");
            Master.HandleError(ex, myCommand2, "look up application record: ");
         }
         catch
         {
            this.lblErrMsg.Text += "Unable to complete request. Please contact your supervisor.<br />";
            this.lblErrMsg.Text += ex.Message + "<br />";
         }
      }
      finally
      {
         if (myReader1 != null && !myReader1.IsClosed)
         {
            myReader1.Close();
         }
         if (myReader2 != null && !myReader2.IsClosed)
         {
            myReader2.Close();
         }

         try
         {
            myCommand1.Dispose();
            myCommand2.Dispose();
         }
         catch (Exception)
         {
         }

         myConnection.Close();
         myConnection.Dispose();
      }
   }
   
   protected void btnAddAgent_Click(object sender, EventArgs e)
   {
      this.lblErrMsg.Text = "";
      bool noMatch = true;
      this.btnAddAgent.Enabled = false;

      foreach (RepeaterItem agent in this.rptAgents.Items)
      {
         if (this.drpAgents.SelectedValue == ((HiddenField)agent.FindControl("hidAgent")).Value)
         {
            noMatch = false;
         }
      }

      if (noMatch)
      {
         objDTAgents = (DataTable)Session["s_agents"];

         objDRAgent = objDTAgents.NewRow();
         objDRAgent["agentName"] = this.drpAgents.SelectedItem.Text;
         objDRAgent["agentID"] = this.drpAgents.SelectedValue;
         objDTAgents.Rows.Add(objDRAgent);

         Session["s_agents"] = objDTAgents;

         this.rptAgents.DataSource = objDTAgents;
         this.rptAgents.DataBind();

         this.drpAgents.SelectedIndex = 0;
         this.drpAgents.Focus();
      }
      else
      {
         this.lblErrMsg.Text += "This agent as already been listed.<br />";
      }
   }

   protected void btnAddError_Click(object sender, EventArgs e)
   {
      this.lblErrMsg.Text = "";
      this.btnAddError.Enabled = false;
      bool noMatch = true;

      foreach (RepeaterItem error in this.rptErrors.Items)
      {
         if (this.drpErrors.SelectedValue == ((Label)error.FindControl("lblError")).Text)
         {
            noMatch = false;
         }
      }

      if (noMatch)
      {
         objDTErrors = (DataTable)Session["s_errors"];

         objDRError = objDTErrors.NewRow();
         objDRError["errorReason"] = this.drpErrors.SelectedValue;
         objDTErrors.Rows.Add(objDRError);

         Session["s_errors"] = objDTErrors;

         this.rptErrors.DataSource = objDTErrors;
         this.rptErrors.DataBind();

         this.drpErrors.SelectedIndex = 0;
         this.drpErrors.Focus();
      }
      else
      {
         this.lblErrMsg.Text += "That error has already been listed.<br />";
      }
   }

   protected void drpDepartment_SelectedIndexChanged(object sender, EventArgs e)
   {
      this.lblErrMsg.Text = "";
      this.txtAccountNbr.Enabled = true;
      this.drpStudents.Enabled = true;
      this.chkError.Checked = false;
      this.chkReminder.Checked = false;
      this.txtNbrOfErrors.Text = "1";
      //Session["s_agents"] = null;
      //this.rptAgents.Visible = false;
      //this.rptAgents.DataSource = null;
      this.rptAgents.DataSource = (DataTable)Session["s_agents"];
      makeTables();
      this.rptAgents.DataBind();

      if (this.drpDepartment.SelectedIndex == 0)
      {
         this.drpAgents.DataSource = getAgents(" = 0");
      }
      else
      {
         switch (this.drpDepartment.SelectedValue)
         {
            case "Customer Service":
               this.drpAgents.DataSource = getAgents(" IN (20, 21, 22, 23, 24, 25, 26, 27, 28, 29)");
               break;

            case "Enrollment":
               this.drpAgents.DataSource = getAgents(" IN (30, 31, 32, 33, 34, 35, 37, 38, 39)");
               break;

            case "Day School":
               this.drpAgents.DataSource = getAgents(" = 36");
               break;

            case "Grading":
               this.drpAgents.DataSource = getAgents(" IN (50, 51, 52, 53, 54, 55, 56, 57, 58, 59)");
               break;

            case "Deposit Room":
               this.drpAgents.DataSource = getAgents(" = 80");
               break;
            case "Collections":
               this.drpAgents.DataSource = getAgents(" = 82");
               break;
         }

         if (this.drpDepartment.SelectedValue == "Grading")
         {
            this.lblNbrOfErrors.Visible = true;
            this.txtNbrOfErrors.Visible = true;

            if (String.IsNullOrEmpty(this.txtAccountNbr.Text))
            {
               this.txtNbrOfErrors.Enabled = true;
            }
            else
            {
               this.txtNbrOfErrors.Enabled = false;
            }
         }
         else
         {
            this.lblNbrOfErrors.Visible = false;
            this.txtNbrOfErrors.Visible = false;
         }
      }

      this.drpAgents.DataBind();
   }

   protected void drpErrors_SelectedIndexChanged(object sender, EventArgs e)
   {
      if (!this.chkError.Checked && !this.chkReminder.Checked)
      {
         this.chkError.Checked = false;
         this.chkReminder.Checked = false;

         lookupError();
      }

      if (this.drpErrors.SelectedIndex > 0)
      {
         this.btnAddError.Enabled = true;
      }
      else
      {
         this.btnAddError.Enabled = false;
      }
   }

   protected void txtNbrOfErrors_TextChanged(object sender, EventArgs e)
   {
      this.lblErrMsg.Text = "";

      if (Convert.ToInt64(this.txtNbrOfErrors.Text) > 1)
      {
         this.txtAccountNbr.Enabled = false;
         this.txtAccountNbr.Text = "";
         this.drpStudents.Enabled = false;
         this.drpStudents.SelectedIndex = 0;
         this.btnAddAgent.Enabled = false;
         if (hasAgents())
         {
            this.drpAgents.SelectedValue = ((HiddenField)this.rptAgents.Controls[1].FindControl("hidAgent")).Value;
            this.rptAgents.DataSource = null;
            this.rptAgents.DataBind();
            Session["s_agents"] = null;
            makeTables();
         }
      }
      else if (this.txtNbrOfErrors.Text == "1")
      {
         this.txtAccountNbr.Enabled = true;
         this.drpStudents.Enabled = true;
         this.btnAddAgent.Enabled = true;
      }
      else
      {
         this.lblErrMsg.Text += "Number of errors cannot be less than one";
         this.txtNbrOfErrors.Text = "1";
         this.txtAccountNbr.Enabled = true;
         this.drpStudents.Enabled = true;
         this.btnAddAgent.Enabled = true;
      }
   }

   protected void txtMoney_TextChanged(object sender, EventArgs e)
   {
      Double money;
      Double.TryParse(this.txtMoney.Text, out money);
      money = Convert.ToDouble(this.txtMoney.Text);

      if (money > 250.00)
      {
         this.chkError.Checked = true;
         this.chkReminder.Checked = false;
      }
      else
      {
         this.chkError.Checked = false;
         this.chkReminder.Checked = false;

         lookupError();
      }
   }

   protected void drpAgents_SelectedIndexChanged(object sender, EventArgs e)
   {
      if (this.drpAgents.SelectedIndex != 0 && this.txtNbrOfErrors.Text == "1")
      {
         this.btnAddAgent.Enabled = true;
      }
      else
      {
         this.btnAddAgent.Enabled = false;
      }
   }

   protected void rptAgents_ItemDataBound(object sender, EventArgs e)
   {
      Int32 count = 0;

      foreach (RepeaterItem agent in this.rptAgents.Items)
      {
         count++;
      }

      if (count > 0)
      {
         this.rptAgents.Visible = true;
      }
      else
      {
         this.rptAgents.Visible = false;
      }
   }

   protected void rptErrors_ItemDataBound(object sender, EventArgs e)
   {
      Int32 count = 0;

      foreach (RepeaterItem error in this.rptErrors.Items)
      {
         count++;
      }

      if (count > 0)
      {
         this.rptErrors.Visible = true;
      }
      else
      {
         this.rptErrors.Visible = false;
      }
   }

   protected void rptAgents_ItemCommand(object sender, RepeaterCommandEventArgs e)
   {
      if (hasAgents())
      {
         RepeaterItem agent = e.Item;

         objDTAgents = (DataTable)Session["s_agents"];
         objDTAgents.Rows[agent.ItemIndex].Delete();
         objDTAgents.AcceptChanges();
         Session["s_agents"] = objDTAgents;

         this.rptAgents.DataSource = objDTAgents;
         this.rptAgents.DataBind();
      }
   }

   protected void rptErrors_ItemCommand(object sender, RepeaterCommandEventArgs e)
   {
      if (hasErrors())
      {
         RepeaterItem error = e.Item;

         objDTErrors = (DataTable)Session["s_errors"];
         if (objDTErrors.Rows.Count > 0)
         {
            objDTErrors.Rows[error.ItemIndex].Delete();
            objDTErrors.AcceptChanges();
            Session["s_errors"] = objDTErrors;
         }
         this.rptErrors.DataSource = objDTErrors;
         this.rptErrors.DataBind();
      }
   }

   protected void lookupError()
   {
      string queryString = "SELECT ag_priority FROM aba.tager WHERE ag_reason = :ag_reason";
      OracleConnection myConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ABAProgrammerConnection"].ToString());
      OracleCommand myCommand = new OracleCommand(queryString, myConnection);
      myCommand.BindByName = true;
      OracleDataReader myReader = default(OracleDataReader);

      try
      {
         myConnection.Open();

         myCommand.Parameters.Clear();
         myCommand.Parameters.Add("ag_reason", this.drpErrors.SelectedValue);

         myReader = myCommand.ExecuteReader();
         myReader.Read();
         if (myReader.HasRows)
         {
            if (myReader["ag_priority"].ToString() == "R")
            {
               this.chkReminder.Checked = true;
            }
            if (myReader["ag_priority"].ToString() == "E")
            {
               this.chkError.Checked = true;
            }
         }
         myReader.Close();
      }
      catch (Exception ex)
      {
         this.lblErrMsg.Text += "ERROR looking up the error information<br />";
         try
         {
            Master.HandleError(ex, myCommand, "look up the error information: ");
         }
         catch
         {
            this.lblErrMsg.Text += "Unable to complete request. Please contact your supervisor.<br />";
            this.lblErrMsg.Text += ex.Message + "<br />";
         }
      }
      finally
      {
         if (myReader != null && !myReader.IsClosed)
         {
            myReader.Close();
         }

         try
         {
            myCommand.Dispose();
         }
         catch (Exception)
         {
         }

         myConnection.Close();
         myConnection.Dispose();
      }
   }

   protected void btnSubmit_Click(object sender, EventArgs e)
   {
      this.lblErrMsg.Text = "";

      validate();
      if (String.IsNullOrEmpty(this.lblErrMsg.Text))
      {
         bool hasMatch = false;

         foreach (RepeaterItem error in this.rptErrors.Items)
         {
            if (this.drpErrors.SelectedValue == ((Label)error.FindControl("lblError")).Text)
            {
               hasMatch = true;
            }
         }

         if (this.drpErrors.SelectedIndex > 0 && !hasMatch)
         {
            send(this.drpErrors.SelectedValue);
         }
         if (hasErrors())
         {
            foreach (RepeaterItem error in this.rptErrors.Items)
            {
               send(((Label)error.FindControl("lblError")).Text);
            }
         }

         clear();
         this.rptAgents.DataSource = null;
         makeTables();
         this.txtAccountNbr.Focus();
      }
   }

   protected void btnClear_Click(object sender, EventArgs e)
   {
      clear();
      this.rptAgents.DataSource = null;
      this.rptErrors.DataSource = null;
      makeTables();
      this.rptAgents.DataBind();
      this.rptErrors.DataBind();
   }

   protected void makeTables()
   {
      objDTAgents = new DataTable("agents");
      objDTErrors = new DataTable("errors");

      objDTAgents.Columns.Add("agentName", typeof(String));
      objDTAgents.Columns.Add("agentID", typeof(String));
      objDTErrors.Columns.Add("errorReason", typeof(String));

      if (Session["s_agents"] == null || String.IsNullOrEmpty(Session["s_agents"].ToString()))
      {
         Session["s_agents"] = objDTAgents;
      }
      if (Session["s_errors"] == null || String.IsNullOrEmpty(Session["s_errors"].ToString()))
      {
         Session["s_errors"] = objDTErrors;
      }
   }

   protected DataTable getAgents(String condition)
   {
      DataTable agents = new DataTable();

      string queryString = @"SELECT utilities.format.propername(u.sec_f_name || ' ' || u.sec_l_name) AS sec_name, g.sec_id 
                               FROM aba.sugrp g 
                               JOIN aba.suser u 
                                 ON u.sec_id = g.sec_id 
                                AND (to_number(to_char(sysdate, 'yyyymmdd')) - u.sec_dis_dt <= 365 OR u.sec_disabl = 'N') 
                              WHERE g.sec_group" + condition + @"
                              GROUP BY u.sec_f_name, u.sec_l_name, g.sec_id
                              ORDER BY UPPER(sec_l_name)";
      OracleConnection myConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ABAProgrammerConnection"].ToString());
      OracleCommand myCommand = new OracleCommand(queryString, myConnection);

      try
      {
         myConnection.Open();

         agents.Load(myCommand.ExecuteReader());
      }
      catch (Exception ex)
      {
         this.lblErrMsg.Text += "ERROR looking up the agents<br />";

         try
         {
            Master.HandleError(ex, myCommand, "look up the agents: ");
         }
         catch
         {
            this.lblErrMsg.Text += "Unable to complete request. Please contact your supervisor.<br />";
            this.lblErrMsg.Text += ex.Message + "<br />";
         }
      }
      finally
      {
         try
         {
            myCommand.Dispose();
         }
         catch (Exception)
         {
         }

         myConnection.Close();
         myConnection.Dispose();
      }

      return agents;
   }

   protected void clear()
   {
      this.lblErrMsg.Text = "";
      this.drpDepartment.SelectedIndex = 0;
      this.txtAccountNbr.Text = "";
      this.txtAccountNbr.Enabled = true;
      this.lblAccountName.Text = "";
      this.drpStudents.SelectedIndex = 0;
      this.drpStudents.Enabled = true;
      this.txtOtherApp.Visible = false;
      this.txtErrorDt.Text = "";
      this.txtMoney.Text = "";
      this.txtComments.Text = "";
      this.chkError.Checked = false;
      this.chkReminder.Checked = false;
      this.drpErrors.SelectedIndex = 0;
      this.lblNbrOfErrors.Visible = false;
      this.txtNbrOfErrors.Text = "1";
      this.txtNbrOfErrors.Visible = false;
      this.drpAgents.DataSource = getAgents(" = 0");
      this.drpAgents.DataBind();
      this.btnAddAgent.Enabled = false;
      this.rptAgents.Visible = false;
      this.btnAddError.Enabled = false;
      this.rptErrors.Visible = false;
      Session.Remove("s_errors");
      Session.Remove("s_agents");
   }

   protected void validate()
   {
      DateTime date;

      if (String.IsNullOrEmpty(this.txtAccountNbr.Text) && this.txtNbrOfErrors.Text == "1")
      {
         this.lblErrMsg.Text += "Please enter an account number.<br />";
      }
      else if (this.drpStudents.SelectedItem.Text == "Other" && this.txtNbrOfErrors.Text == "1")
      {
         if (String.IsNullOrEmpty(this.txtOtherApp.Text))
         {
            this.lblErrMsg.Text += "Please enter an application number.<br />";
         }
      }
      else
      {
         if (this.drpDepartment.SelectedIndex == 0)
         {
            this.lblErrMsg.Text += "Please select the area/department.<br />";
         }
         else
         {
            if (!hasErrors() && this.drpErrors.SelectedIndex == 0)
            {
               this.lblErrMsg.Text += "Please select the type of error.<br />";
            }
            if (!hasAgents() && this.drpAgents.SelectedIndex == 0)
            {
               this.lblErrMsg.Text += "Please select an agent.<br />";
            }
            else if (this.drpErrors.SelectedValue == "Other")
            {
               if (!this.chkError.Checked && !this.chkReminder.Checked)
               {
                  this.lblErrMsg.Text += "Please select whether this is an Error or a Reminder.<br />";
               }
               if (String.IsNullOrEmpty(this.txtComments.Text))
               {
                  this.lblErrMsg.Text += "Please enter an explanation or any comments.<br />";
               }
            }
         }
         if (String.IsNullOrEmpty(this.txtErrorDt.Text))
         {
            this.lblErrMsg.Text += "Please enter the error date.<br />";
         }
         else if (!DateTime.TryParseExact(this.txtErrorDt.Text, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
         {
            this.lblErrMsg.Text = "Please enter a valid error date.<br />";
         }
         else if (Convert.ToInt64(this.txtErrorDt.Text) > Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")))
         {
            this.lblErrMsg.Text += "Error date cannot be in the future.<br />";
         }
      }
   }

   protected bool hasAgents()
   {
      int count = 0;

      foreach (RepeaterItem agent in this.rptAgents.Items)
      {
         count++;
      }

      return count != 0;
   }

   protected bool hasErrors()
   {
      int count = 0;

      foreach (RepeaterItem error in this.rptErrors.Items)
      {
         count++;
      }

      return count != 0;
   }

   protected void send(String error)
   {
      string queryString1 = "INSERT INTO aba.agenterrors (agentid, accountnumber, applicationnumber, error_date, department, errortype, numberoferrors, comments, enter_date, money, entered_by, priority) VALUES (:agentid, :accountnumber, :applicationnumber, to_date(:error_date, 'yyyyMMdd'), :department, :errortype, :numberoferrors, :comments, sysdate, :money, :entered_by, :priority)";
      OracleConnection myConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ABAProgrammerConnection"].ToString());
      OracleCommand myCommand1 = new OracleCommand(queryString1, myConnection);
      myCommand1.BindByName = true;
      OracleTransaction myTransaction = default(OracleTransaction);

      string queryString2 = "SELECT wn_shortnm FROM aba.wname WHERE wn_name = :wn_name";
      OracleCommand myCommand2 = new OracleCommand(queryString2, myConnection);
      myCommand2.BindByName = true;

      string queryString3 = "SELECT NVL((to_number(MAX(uc_key)) + 1), 1) AS uc_key FROM aba.ucomm WHERE uc_user_id = :uc_user_id";
      OracleCommand myCommand3 = new OracleCommand(queryString3, myConnection);
      myCommand3.BindByName = true;

      string queryString4 = "INSERT INTO aba.ucomm (uc_crte_by, uc_user_id, uc_key, uc_crte_page, uc_comment, uc_lineno, uc_crte_dt, uc_end_dt) VALUES (:uc_crte_by, :uc_user_id, :uc_key, :uc_crte_page, :uc_comment, :uc_lineno, to_char(sysdate, 'yyyymmdd'), to_char(add_months(sysdate, 6), 'yyyymmdd'))";
      OracleCommand myCommand4 = new OracleCommand(queryString4, myConnection);
      myCommand4.BindByName = true;

      String pageName = "";
      String comment = "",
         origComment = (this.chkError.Checked ? "Error" : "") + (this.chkReminder.Checked ? "Reminder" : "") + ": " + error + "<br />Date: " + DateTime.ParseExact(this.txtErrorDt.Text, "yyyyMMdd", CultureInfo.InvariantCulture).ToLongDateString() + (!String.IsNullOrEmpty(this.txtAccountNbr.Text) ? "<br />Account: " + this.txtAccountNbr.Text : "") + (this.drpStudents.SelectedIndex > 0 ? "<br />Application: " + (this.drpStudents.SelectedItem.Text == "Other" ? this.txtOtherApp.Text : this.drpStudents.SelectedValue) : "") + (Convert.ToInt64(this.txtNbrOfErrors.Text) > 1 ? "<br />Number of Times: " + this.txtNbrOfErrors.Text : "") + (!String.IsNullOrEmpty(this.txtMoney.Text) ? "<br />Amount: $" + this.txtMoney.Text : "") + (!String.IsNullOrEmpty(this.txtComments.Text) ? "<br />Comments: " + this.txtComments.Text : "");
      String key = "1";
      Int32 count = 1,
            max = origComment.Length / 50,
            end = origComment.Length - (max * 50);

      try
      {
         myConnection.Open();
         myTransaction = myConnection.BeginTransaction();

         bool hasMatch = false;
         foreach (RepeaterItem agent in this.rptAgents.Items)
         {
            if (this.drpAgents.SelectedValue == ((HiddenField)agent.FindControl("hidAgent")).Value)
            {
               hasMatch = true;
            }
         }

         if (this.drpAgents.SelectedIndex > 0 && !hasMatch)
         {
            myCommand1.Transaction = myTransaction;
            myCommand1.Parameters.Clear();
            myCommand1.Parameters.Add("agentid", this.drpAgents.SelectedValue);
            myCommand1.Parameters.Add("accountnumber", this.txtAccountNbr.Text);
            myCommand1.Parameters.Add("applicationnumber", (this.drpStudents.SelectedItem.Text == "Other" ? this.txtOtherApp.Text : this.drpStudents.SelectedValue));
            myCommand1.Parameters.Add("error_date", this.txtErrorDt.Text);
            myCommand1.Parameters.Add("department", this.drpDepartment.SelectedValue);
            myCommand1.Parameters.Add("errortype", error);
            myCommand1.Parameters.Add("numberoferrors", this.txtNbrOfErrors.Text);
            myCommand1.Parameters.Add("comments", this.txtComments.Text);
            myCommand1.Parameters.Add("money", this.txtMoney.Text);
            myCommand1.Parameters.Add("entered_by", Session["abadbUserID"].ToString());
            myCommand1.Parameters.Add("priority", (this.chkError.Checked ? "E" : "R"));
            myCommand1.ExecuteNonQuery();

            myCommand2.Parameters.Clear();
            myCommand2.Parameters.Add("wn_name", Page.Title.ToUpper());
            pageName = myCommand2.ExecuteScalar().ToString();

            myCommand3.Parameters.Clear();
            myCommand3.Parameters.Add("uc_user_id", this.drpAgents.SelectedValue);
            key = myCommand3.ExecuteScalar().ToString();

            myCommand4.Transaction = myTransaction;
            if (max == 0)
            {
               comment = origComment.Substring(0, end);

               myCommand4.Parameters.Clear();
               myCommand4.Parameters.Add("uc_crte_by", Session["abadbUserID"].ToString());
               myCommand4.Parameters.Add("uc_user_id", this.drpAgents.SelectedValue);
               myCommand4.Parameters.Add("uc_key", key);
               myCommand4.Parameters.Add("uc_crte_page", pageName);
               myCommand4.Parameters.Add("uc_comment", comment);
               myCommand4.Parameters.Add("uc_lineno", count);
               myCommand4.ExecuteNonQuery();
            }
            else
            {
               comment = origComment.Substring(0, 50);

               myCommand4.Parameters.Clear();
               myCommand4.Parameters.Add("uc_crte_by", Session["abadbUserID"].ToString());
               myCommand4.Parameters.Add("uc_user_id", this.drpAgents.SelectedValue);
               myCommand4.Parameters.Add("uc_key", key);
               myCommand4.Parameters.Add("uc_crte_page", pageName);
               myCommand4.Parameters.Add("uc_comment", comment);
               myCommand4.Parameters.Add("uc_lineno", count);
               myCommand4.ExecuteNonQuery();

               while (count < max)
               {
                  comment = origComment.Substring(count * 50, 50);
                  count++;

                  myCommand4.Parameters.Clear();
                  myCommand4.Parameters.Add("uc_crte_by", Session["abadbUserID"].ToString());
                  myCommand4.Parameters.Add("uc_user_id", this.drpAgents.SelectedValue);
                  myCommand4.Parameters.Add("uc_key", key);
                  myCommand4.Parameters.Add("uc_crte_page", pageName);
                  myCommand4.Parameters.Add("uc_comment", comment);
                  myCommand4.Parameters.Add("uc_lineno", count);
                  myCommand4.ExecuteNonQuery();
               }
               if (end != 0)
               {
                  comment = origComment.Substring(count * 50, end);

                  myCommand4.Parameters.Clear();
                  myCommand4.Parameters.Add("uc_crte_by", Session["abadbUserID"].ToString());
                  myCommand4.Parameters.Add("uc_user_id", this.drpAgents.SelectedValue);
                  myCommand4.Parameters.Add("uc_key", key);
                  myCommand4.Parameters.Add("uc_crte_page", pageName);
                  myCommand4.Parameters.Add("uc_comment", comment);
                  myCommand4.Parameters.Add("uc_lineno", count + 1);
                  myCommand4.ExecuteNonQuery();
               }
            }
         }

         if (hasAgents() && this.txtNbrOfErrors.Text == "1")
         {
            foreach (RepeaterItem agent in this.rptAgents.Items)
            {
               count = 1;
               key = "1";

               myCommand1.Transaction = myTransaction;
               myCommand1.Parameters.Clear();
               myCommand1.Parameters.Add("agentid", ((HiddenField)agent.FindControl("hidAgent")).Value);
               myCommand1.Parameters.Add("accountnumber", this.txtAccountNbr.Text);
               myCommand1.Parameters.Add("applicationnumber", (this.drpStudents.SelectedItem.Text == "Other" ? this.txtOtherApp.Text : this.drpStudents.SelectedValue));
               myCommand1.Parameters.Add("error_date", this.txtErrorDt.Text);
               myCommand1.Parameters.Add("department", this.drpDepartment.SelectedValue);
               myCommand1.Parameters.Add("errortype", error);
               myCommand1.Parameters.Add("numberoferrors", this.txtNbrOfErrors.Text);
               myCommand1.Parameters.Add("comments", this.txtComments.Text);
               myCommand1.Parameters.Add("money", this.txtMoney.Text);
               myCommand1.Parameters.Add("entered_by", Session["abadbUserID"].ToString());
               myCommand1.Parameters.Add("priority", (this.chkError.Checked ? "E" : "R"));
               myCommand1.ExecuteNonQuery();

               myCommand2.Parameters.Clear();
               myCommand2.Parameters.Add("wn_name", Page.Title.ToUpper());
               pageName = myCommand2.ExecuteScalar().ToString();

               myCommand3.Parameters.Clear();
               myCommand3.Parameters.Add("uc_user_id", ((HiddenField)agent.FindControl("hidAgent")).Value);
               key = myCommand3.ExecuteScalar().ToString();

               myCommand4.Transaction = myTransaction;
               if (max == 0)
               {
                  comment = origComment.Substring(0, end);

                  myCommand4.Parameters.Clear();
                  myCommand4.Parameters.Add("uc_crte_by", Session["abadbUserID"].ToString());
                  myCommand4.Parameters.Add("uc_user_id", ((HiddenField)agent.FindControl("hidAgent")).Value);
                  myCommand4.Parameters.Add("uc_key", key);
                  myCommand4.Parameters.Add("uc_comment", comment);
                  myCommand4.Parameters.Add("uc_lineno", count);
                  myCommand4.ExecuteNonQuery();
               }
               else
               {
                  comment = origComment.Substring(0, 50);

                  myCommand4.Parameters.Clear();
                  myCommand4.Parameters.Add("uc_crte_by", Session["abadbUserID"].ToString());
                  myCommand4.Parameters.Add("uc_user_id", ((HiddenField)agent.FindControl("hidAgent")).Value);
                  myCommand4.Parameters.Add("uc_key", key);
                  myCommand4.Parameters.Add("uc_crte_page", pageName);
                  myCommand4.Parameters.Add("uc_comment", comment);
                  myCommand4.Parameters.Add("uc_lineno", count);
                  myCommand4.ExecuteNonQuery();

                  while (count < max)
                  {
                     comment = origComment.Substring(count * 50, 50);
                     count++;

                     myCommand4.Parameters.Clear();
                     myCommand4.Parameters.Add("uc_crte_by", Session["abadbUserID"].ToString());
                     myCommand4.Parameters.Add("uc_user_id", ((HiddenField)agent.FindControl("hidAgent")).Value);
                     myCommand4.Parameters.Add("uc_key", key);
                     myCommand4.Parameters.Add("uc_crte_page", pageName);
                     myCommand4.Parameters.Add("uc_comment", comment);
                     myCommand4.Parameters.Add("uc_lineno", count);
                     myCommand4.ExecuteNonQuery();
                  }
                  if (max != 0)
                  {
                     comment = origComment.Substring(count * 50, end);

                     myCommand4.Parameters.Clear();
                     myCommand4.Parameters.Add("uc_crte_by", Session["abadbUserID"].ToString());
                     myCommand4.Parameters.Add("uc_user_id", ((HiddenField)agent.FindControl("hidAgent")).Value);
                     myCommand4.Parameters.Add("uc_key", key);
                     myCommand4.Parameters.Add("uc_crte_page", pageName);
                     myCommand4.Parameters.Add("uc_comment", comment);
                     myCommand4.Parameters.Add("uc_lineno", count + 1);
                     myCommand4.ExecuteNonQuery();
                  }
               }
            }
         }

         myTransaction.Commit();
      }
      catch (Exception ex)
      {
         myTransaction.Rollback();

         this.lblErrMsg.Text += "ERROR submitting the error report<br />";
         try
         {
            Master.HandleError(ex, myCommand1, "submit the error report: ");
            Master.HandleError(ex, myCommand4, "submit the error report: ");
         }
         catch
         {
            this.lblErrMsg.Text += "Unable to complete request. Please contact your supervisor.<br />";
            this.lblErrMsg.Text += ex.Message + "<br />";
         }
      }
      finally
      {
         try
         {
            myCommand1.Dispose();
            myCommand2.Dispose();
            myCommand3.Dispose();
            myCommand4.Dispose();
         }
         catch (Exception)
         {
         }

         myTransaction.Dispose();
         myConnection.Close();
         myConnection.Dispose();
      }
   }

   protected void sqlSelecting(object sender, SqlDataSourceSelectingEventArgs e)
   {
      ((OracleCommand)e.Command).BindByName = true;
   }

   protected void drpAgents_DataBinding(object sender, EventArgs e)
   {
      this.drpAgents.Items.Clear();
      this.drpAgents.Items.Add(" ");
   }
}