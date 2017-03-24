<%@ Page Title="Agent Error Report" Language="C#" MasterPageFile="~/ABAWeb/MasterPages/ABAWeb.master" AutoEventWireup="true" CodeFile="AgentError.aspx.cs" Inherits="ABAWeb_ABA_Screens_AgentError" %>
<%@ MasterType VirtualPath="~/abaweb/MasterPages/ABAWeb.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
   <asp:SqlDataSource ID="sqlStudents" runat="server" 
                      ConnectionString="<%$ ConnectionStrings:ABAProgrammerConnection %>"
                      ProviderName="<%$ ConnectionStrings:ABAProgrammerConnection.ProviderName %>"
                      SelectCommand="SELECT ' ' AS appDesc, 9999999 AS ap_apref, 9999999 AS fd_id 
                                       FROM dual 
                                      UNION SELECT trim(to_char(ap_apref)) || ' - ' || CASE WHEN INSTR(f_name, ' ') <> 0 THEN SUBSTR(f_name, 1, INSTR(f_name, ' ')) ELSE f_name END || ', ' || DECODE(ap_grade, 14, 'K4', 15, 'K5', ap_grade) AS appDesc, ap_apref, fd_id 
                                       FROM aba.appld, aba.stdnt 
                                      WHERE fd_host_no = :fd_host_no 
                                        AND fd_host_no &gt; 0 
                                        AND ap_type &lt;= 10 
                                        AND fd_id = s_id 
                                        AND ap_beg_dt &gt;= s_begin_dt 
                                        AND hs_diploma &lt;&gt; 'G' 
                                      UNION SELECT 'Other' AS appDesc, 0 AS ap_apref, 0 AS fd_id 
                                       FROM dual ORDER BY ap_apref desc, fd_id"
                      OnSelecting="sqlSelecting">
      <SelectParameters>
         <asp:ControlParameter ControlID="txtAccountNbr" Name="fd_host_no" DefaultValue="0" />
      </SelectParameters>
   </asp:SqlDataSource>
   <asp:SqlDataSource ID="sqlErrors" runat="server"
                      ConnectionString="<%$ ConnectionStrings:ABAProgrammerConnection %>"
                      ProviderName="<%$ ConnectionStrings:ABAProgrammerConnection.ProviderName %>"
                      SelectCommand="SELECT ' ' AS ag_reason, 0 AS ag_key FROM dual UNION SELECT ag_reason, ag_key FROM aba.tager WHERE ag_department = :ag_department AND maint <> 'D' OR ag_department = 'All' UNION SELECT 'Other' AS ag_reason, 999 AS ag_key FROM dual ORDER BY ag_key"
                      OnSelecting="sqlSelecting">
      <SelectParameters>
         <asp:ControlParameter ControlID="drpDepartment" Name="ag_department" DefaultValue="0" />
      </SelectParameters>
   </asp:SqlDataSource>

   <asp:Panel runat="server" DefaultButton="btnSubmit">
      <table>
         <tr>
            <td>
               <table>
                  <tr>
                     <td align="right">
                        Account:
                     </td>
                     <td>
                        <asp:TextBox ID="txtAccountNbr" runat="server" MaxLength="7" Width="53px" OnFocus='this.select()' AutoPostBack="true" OnTextChanged="txtAccountNbr_TextChanged" />
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtAccountNbr" FilterType="Numbers" />
                        <asp:Label ID="lblAccountName" runat="server" />
                     </td>
                     <td>
                        &nbsp;&nbsp;
                     </td>
                     <td align="right">
                        Student App:
                     </td>
                     <td>
                        <asp:DropDownList ID="drpStudents" runat="server" DataSourceID="sqlStudents" DataTextField="appDesc" DataValueField="ap_apref" AutoPostBack="true" OnSelectedIndexChanged="drpStudents_SelectedIndexChanged" />
                        <asp:TextBox ID="txtOtherApp" runat="server" MaxLength="7" Width="53px" AutoPostBack="true" OnTextChanged="txtOtherApp_TextChanged" Visible="false" />
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextboxExtender2" runat="server" TargetControlID="txtOtherApp" FilterType="Numbers" />
                     </td>
                  </tr>
               </table>
            </td>
            <td align="right">
               <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
            </td>
         </tr>
         <tr>
            <td colspan="2" valign="top">
               <table>
                  <tr>
                     <td>
                        Area/Department:
                     </td>
                     <td rowspan="4">
                        &nbsp;&nbsp;&nbsp;
                     </td>
                  </tr>
                  <tr>
                     <td valign="top">
                        <asp:DropDownList ID="drpDepartment" runat="server" AutoPostBack="true" onselectedindexchanged="drpDepartment_SelectedIndexChanged">
                           <asp:ListItem Text=" " Value=" " />
                           <asp:ListItem Text="Customer Service" Value="Customer Service" />
                           <asp:ListItem Text="Enrollment" Value="Enrollment" />
                           <asp:ListItem Text="Day School" Value="Day School" />
                           <asp:ListItem Text="Grading" Value="Grading" />
                           <asp:ListItem Text="Deposit Room" Value="Deposit Room" />
                           <asp:ListItem Text="Collections" Value="Collections" />
                        </asp:DropDownList>
                     </td>
                     <td colspan="2" rowspan="2">
                        <asp:CheckBox ID="chkReminder" runat="server" Text="Reminder" TextAlign="Right" />
                        <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender1" runat="server" TargetControlID="chkReminder" Key="Priority" />
                        <br />
                        <asp:CheckBox ID="chkError" runat="server" Text="Error" TextAlign="Right" />
                        <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender2" runat="server" TargetControlID="chkError" Key="Priority" />
                     </td>
                     <td rowspan="2">
                        &nbsp;&nbsp;
                     </td>
                     <td align="right" rowspan="2">
                        <asp:Label ID="lblNbrOfErrors" runat="server" Text="Number&nbsp;&nbsp;<br />of Errors:" style="text-align:center" />
                     </td>
                     <td valign="bottom" rowspan="2" style="padding-bottom:5px">
                        <asp:TextBox ID="txtNbrOfErrors" runat="server" MaxLength="2" Width="18px" AutoPostBack="true" OnTextChanged="txtNbrOfErrors_TextChanged" style="text-align:right" />
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtNbrOfErrors" FilterType="Numbers" />
                     </td>
                  </tr>
                  <tr>
                     <td>
                        Type of Error:
                     </td>
                  </tr>
                  <tr>
                     <td valign="top">
                        <asp:DropDownList ID="drpErrors" runat="server" DataSourceID="sqlErrors" DataTextField="ag_reason" DataValueField="ag_reason" AutoPostBack="true" onselectedindexchanged="drpErrors_SelectedIndexChanged" />
                        <asp:Button ID="btnAddError" runat="server" Text="Add" OnClick="btnAddError_Click" />
                     </td>
                     <td align="right">
                        Amount:
                     </td>
                     <td align="right">
                        $
                     </td>
                     <td colspan="3">
                        <asp:TextBox ID="txtMoney" runat="server" AutoPostBack="true" OnTextChanged="txtMoney_TextChanged" MaxLength="7" Width="48px" />
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtMoney" FilterType="Custom, Numbers" ValidChars="." />
                     </td>
                  </tr>
                  <tr>
                     <td align="right">
                        <asp:Repeater ID="rptErrors" runat="server" OnItemDataBound="rptErrors_ItemDataBound" OnItemCommand="rptErrors_ItemCommand" Visible="false">
                           <HeaderTemplate>
                              <table>
                           </HeaderTemplate>
                           <ItemTemplate>
                                 <tr>
                                    <td>
                                       <asp:Label ID="lblError" runat="server" Text='<%# Eval("errorReason") %>' />
                                    </td>
                                    <td width="50px">
                                       <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" CommandArgument='<%# Container.ItemIndex %>' />
                                    </td>
                                 </tr>
                           </ItemTemplate>
                           <FooterTemplate>
                              </table>
                           </FooterTemplate>
                        </asp:Repeater>
                     </td>
                  </tr>
               </table>
            </td>
         </tr>
         <tr>
            <td colspan="2">
               <table>
                  <tr>
                     <td align="right">
                        Agent:
                     </td>
                     <td valign="top">
                        <asp:DropDownList ID="drpAgents" runat="server" DataTextField="sec_name" 
                           DataValueField="sec_id" AutoPostBack="true" 
                           OnSelectedIndexChanged="drpAgents_SelectedIndexChanged" 
                           AppendDataBoundItems="True" ondatabinding="drpAgents_DataBinding" />
                        <asp:Button ID="btnAddAgent" runat="server" Text="Add" OnClick="btnAddAgent_Click" />
                     </td>
                  </tr>
                  <tr>
                     <td colspan="2" align="right">
                        <asp:Repeater ID="rptAgents" runat="server" OnItemDataBound="rptAgents_ItemDataBound" OnItemCommand="rptAgents_ItemCommand" Visible="false">
                           <HeaderTemplate>
                              <table>
                           </HeaderTemplate>
                           <ItemTemplate>
                                 <tr>
                                    <td>
                                       <asp:Label ID="lblAgent" runat="server" Text='<%# Eval("agentName") %>' />
                                       <asp:HiddenField ID="hidAgent" runat="server" Value='<%# Eval("agentID") %>' />
                                    </td>
                                    <td width="50px">
                                       <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" CommandArgument='<%# Container.ItemIndex %>' />
                                    </td>
                                 </tr>
                           </ItemTemplate>
                           <FooterTemplate>
                              </table>
                           </FooterTemplate>
                        </asp:Repeater>
                     </td>
                  </tr>
                  <tr>
                     <td align="right">
                        Date of Error:
                     </td>
                     <td>
                        <asp:TextBox ID="txtErrorDt" runat="server" Width="60px" />
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtErrorDt" FilterType="Numbers" />
                        <asp:ImageButton ID="imgCalendar" ImageUrl="~/images/calendar.png" ImageAlign="TextTop" runat="server" style="margin-left: 3px;" />
                        <ajaxToolkit:CalendarExtender ID="cleErrorDt" TargetControlID="txtErrorDt" PopupButtonID="imgCalendar" Format="yyyyMMdd" CssClass="ABACalendarExtender" runat="server" />
                        <asp:Label ID="lblDateFormat" runat="server" Text="(YYYYMMDD)" ForeColor="Gray" Font-Italic="true" Font-Size="Smaller" />
                     </td>
                  </tr>
               </table>
            </td>
         </tr>
         <tr>
            <td>
               <table>
                  <tr>
                     <td>
                        Explanation/Comments:
                     </td>
                  </tr>
                  <tr>
                     <td>
                        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="400px" />
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtComments" FilterMode="InvalidChars" InvalidChars="|" />
                     </td>
                     <td />
                     <td valign="bottom">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" />
                     </td>
                  </tr>
               </table>
            </td>
         </tr>
         <tr>
            <td colspan="2">
               <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" />
            </td>
         </tr>
      </table>
   </asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainFooter" runat="server">
</asp:Content>