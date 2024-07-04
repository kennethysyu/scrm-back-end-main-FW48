using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrm1.Controllers
{
    //public class CustCaseDetails
    //{
    //    public int Customer_Id;
    //    public DateTime Created_Time;
    //    public string Created_By;
    //    public DateTime Updated_Time;
    //    public string ID_No;
    //    public string Name_Chi;
    //    public string Name_Eng;
    //    public string Gender;
    //    public string Title;
    //    public string Lang;
    //    public DateTime DOB;
    //    public string Home_No;
    //    public string Office_No;
    //    public string Mobile_No;
    //    public string Fax_No;
    //    public string Other_Phone_No;
    //    public string Email;
    //    public string Address1;
    //    public string Address2;
    //    public string Address3;
    //    public string Address4;
    //    public string Full_Address;
    //    public string Occupation;
    //    public int? Case_No;
    //    public string Call_Nature;
    //    public string Details;
    //    public string Status;
    //    public string Remarks;
    //    public string Agent;
    //    public DateTime Creation_Date_Time;
    //}

    //public class CustomerDetails
    //{
    //    public bool have_case;
    //    public int Customer_Id;
    //    public DateTime Created_Time;
    //    public string Created_By;
    //    public DateTime Updated_Time;
    //    public string ID_No;
    //    public string Name_Chi;
    //    public string Name_Eng;
    //    public string Gender;
    //    public string Title;
    //    public string Lang;
    //    public DateTime DOB;
    //    public string Home_No;
    //    public string Office_No;
    //    public string Mobile_No;
    //    public string Fax_No;
    //    public string Other_Phone_No;
    //    public string Email;
    //    public string Address1;
    //    public string Address2;
    //    public string Address3;
    //    public string Address4;
    //    public string Full_Address;
    //    public string Occupation;
    //}

    //public class CaseDetails
    //{
    //    public int? Case_No;
    //    public string Call_Nature;
    //    public string Details;
    //    public string Status;
    //    public string Remarks;
    //    public string Agent;
    //    public DateTime Creation_Date_Time;
    //}

    public class dummy
    {
        //    public DataContractJsonSerializer getCustSearchResultsTest([FromBody] dynamic data, string searchType)
        //    {
        //        // obtain a list of data from table "case_result"
        //        List<case_result_list_item> _linq_case = (from _v in _scrme.case_result
        //                                                  select _v).ToList();

        //        // declare a list of contact_list_item data
        //        List<contact_list_item> _search_result_by_details = new List<contact_list_item>();

        //        bool _have_case = false;

        //        // search by different types of search: auto and manual
        //        if (searchType == "auto")
        //        {
        //            // call autoSearch function
        //            _search_result_by_details = autoSearch(data);
        //        }
        //        else if (searchType == "manual")
        //        {
        //            // call manualSearch function
        //            _search_result_by_details = manualSearch(data);
        //        }

        //        MemoryStream str = new MemoryStream();
        //        DataContractJsonSerializer _all_results = new DataContractJsonSerializer(_search_result_by_details.GetType());


        //        if (_search_result_by_details.Count() > 0)
        //        {

        //        }

        //        return _all_results;

        //    }

        //    // Obtain final results from either AutoSearch or ManualSearch
        //    public List<CustomerDetails> getCustSearchResultsOld([FromBody] dynamic data, string searchType)
        //    {

        //        // obtain a list of data from table "case_result"
        //        List<case_result_list_item> _linq_case = (from _v in _scrme.case_result
        //                                                  select _v).ToList();

        //        // declare a list of contact_list_item data
        //        List<contact_list_item> _search_result_by_details = new List<contact_list_item>();

        //        // declare a list of CustomerDetails objects
        //        List<CustomerDetails> _cust_details = new List<CustomerDetails>();

        //        // search by different types of search: auto and manual
        //        if (searchType == "auto")
        //        {
        //            // call autoSearch function
        //            _search_result_by_details = autoSearch(data);
        //        }
        //        else if (searchType == "manual")
        //        {
        //            // call manualSearch function
        //            _search_result_by_details = manualSearch(data);
        //        }

        //        // find respective cases in each search result item
        //        if (_search_result_by_details.Count() > 0)
        //        {
        //            // iterate through the customers
        //            foreach (contact_list_item _contact_item in _search_result_by_details)
        //            {
        //                // declare CustomerDetails object
        //                CustomerDetails _cust_item = new CustomerDetails();

        //                // retrieve the cases based on the searched customer ids
        //                var _case = _linq_case.Where(_c => _c.Customer_Id == _contact_item.Customer_Id);

        //                // there's at least a case for that particular customer id
        //                if (_case.Count() > 0)
        //                {
        //                    // assign true to "have_case"
        //                    _cust_item.have_case = true;
        //                }
        //                else
        //                {
        //                    _cust_item.have_case = false;
        //                }

        //                // assign the table data to the CustomerDetails class
        //                _cust_item.Customer_Id = _contact_item.Customer_Id;
        //                _cust_item.Created_Time = _contact_item.Created_Time.Value;
        //                _cust_item.Created_By = _contact_item.Created_By.ToString();
        //                _cust_item.Updated_Time = _contact_item.Updated_Time.Value;
        //                _cust_item.ID_No = _contact_item.ID_No;
        //                _cust_item.Name_Chi = _contact_item.Name_Chi;
        //                _cust_item.Name_Eng = _contact_item.Name_Eng;
        //                _cust_item.Gender = _contact_item.Gender;
        //                _cust_item.Title = _contact_item.Title;
        //                _cust_item.Lang = _contact_item.Lang;
        //                _cust_item.DOB = (_contact_item.DOB != null) ? _contact_item.DOB.Value : DateTime.MinValue;
        //                _cust_item.Home_No = _contact_item.Home_No;
        //                _cust_item.Office_No = _contact_item.Office_No;
        //                _cust_item.Mobile_No = _contact_item.Mobile_No;
        //                _cust_item.Fax_No = _contact_item.Fax_No;
        //                _cust_item.Other_Phone_No = _contact_item.Other_Phone_No;
        //                _cust_item.Email = _contact_item.Email;
        //                _cust_item.Address1 = _contact_item.Address1;
        //                _cust_item.Address2 = _contact_item.Address2;
        //                _cust_item.Address3 = _contact_item.Address3;
        //                _cust_item.Address4 = _contact_item.Address4;
        //                _cust_item.Full_Address = _contact_item.Full_Address;
        //                _cust_item.Occupation = _contact_item.Occupation;

        //                // append to list
        //                _cust_details.Add(_cust_item);
        //            }
        //        }
        //        // for no result, assign null to the whole object
        //        else
        //        {
        //            _cust_details = null;
        //        }

        //        return _cust_details;
        //    }

        //public List<CustCaseDetails> getCaseAutoSearchResults([FromBody] dynamic data, string conditions)
        //{
        //    // declare a list of CustCaseDetails
        //    List<CustCaseDetails> _all_cases = new List<CustCaseDetails>();

        //    //// declare search condition
        //    //string condition = "@0.Contains(outerIt._cust.Customer_Id)";

        //    // obtain linq results joining 2 tables: contact_list and case_result
        //    var _joined_results = from _cust in _scrme.contact_list
        //                          join _case in _scrme.case_result
        //                              on _cust.Customer_Id equals _case.Customer_Id
        //                              into _case_group
        //                          from _case in _case_group.DefaultIfEmpty()
        //                          where _case.Case_No != null // for filtering out cases that don't have case no. yet.
        //                          select new
        //                          {
        //                              _cust,
        //                              _case
        //                          };

        //    var _filtered_results_auto = _joined_results;

        //    // Perform the real CaseAutoSearch
        //    if (conditions == "")
        //    {
        //        List<int> _cust_ids = getCustomerIdsFromCases(data);
        //        string auto_condition = "@0.Contains(outerIt._cust.Customer_Id)";

        //        // Filter results from the customer ids obtained from cases
        //        _filtered_results_auto = _joined_results.Where(auto_condition, _cust_ids);
        //    }
        //    else
        //    {
        //        // Filter results from the case manual search criteria
        //        _filtered_results_auto = _joined_results.Where(conditions);
        //    }

        //    // there is at least one result
        //    if (_filtered_results_auto.Count() > 0)
        //    {
        //        // assign results' row values to the CustCaseDetails object
        //        foreach (var _result_item in _filtered_results_auto)
        //        {
        //            // declare CustCaseDetails object
        //            CustCaseDetails _details = new CustCaseDetails();

        //            _details.Customer_Id = _result_item._cust.Customer_Id;
        //            _details.Created_Time = _result_item._cust.Created_Time.Value;
        //            _details.Created_By = _result_item._cust.Created_By.ToString();
        //            _details.Updated_Time = _result_item._cust.Updated_Time.Value;
        //            _details.ID_No = _result_item._cust.ID_No;
        //            _details.Name_Chi = _result_item._cust.Name_Chi;
        //            _details.Name_Eng = _result_item._cust.Name_Eng;
        //            _details.Gender = _result_item._cust.Gender;
        //            _details.Title = _result_item._cust.Title;
        //            _details.Lang = _result_item._cust.Lang;
        //            _details.DOB = (_result_item._cust.DOB != null) ? _result_item._cust.DOB.Value : DateTime.MinValue;
        //            _details.Home_No = _result_item._cust.Home_No;
        //            _details.Office_No = _result_item._cust.Office_No;
        //            _details.Mobile_No = _result_item._cust.Mobile_No;
        //            _details.Fax_No = _result_item._cust.Fax_No;
        //            _details.Other_Phone_No = _result_item._cust.Other_Phone_No;
        //            _details.Email = _result_item._cust.Email;
        //            _details.Address1 = _result_item._cust.Address1;
        //            _details.Address2 = _result_item._cust.Address2;
        //            _details.Address3 = _result_item._cust.Address3;
        //            _details.Address4 = _result_item._cust.Address4;
        //            _details.Full_Address = _result_item._cust.Full_Address;
        //            _details.Occupation = _result_item._cust.Occupation;
        //            _details.Case_No = (_result_item._case.Case_No != null) ? _result_item._case.Case_No.Value : 0;
        //            _details.Call_Nature = _result_item._case.Call_Nature;
        //            _details.Details = _result_item._case.Details;
        //            _details.Status = _result_item._case.Status;
        //            _details.Remarks = _result_item._case.Remark;
        //            _details.Agent = _result_item._case.Created_By.ToString();
        //            _details.Creation_Date_Time = _result_item._case.Created_Time.Value;

        //            _all_cases.Add(_details);

        //        }
        //    }
        //    else
        //    {
        //        _all_cases = null;
        //    }

        //    return _all_cases;
        //}

        //public List<CustCaseDetails> getCaseManualSearchResults([FromBody] dynamic data)
        //{
        //    // declare a list of CustCaseDetails
        //    List<CustCaseDetails> _all_cases = new List<CustCaseDetails>();

        //    // declare search condition
        //    string conditions = getCaseManualSearchConditions(data);

        //    // if the search criteria do not involve the "case_followup" table
        //    if (!conditions.Contains("_followup"))
        //    {
        //        // can call the CAS function
        //        _all_cases = getCaseAutoSearchResults(data, conditions);
        //    }
        //    else
        //    {
        //        // the "case_followup" table has to be joined
        //        var _joined_results = from _cust in _scrme.contact_list
        //                              join _case in _scrme.case_result
        //                                 on _cust.Customer_Id equals _case.Customer_Id
        //                                 into _case_group
        //                              from _case1 in _case_group.DefaultIfEmpty()
        //                                  //where _case1.Case_No != null
        //                              join _followup in _scrme.case_followup
        //                                  on _case1.Case_No equals _followup.Case_No
        //                              select new
        //                              {
        //                                  //Customer = _cust,
        //                                  //Case = _case1,
        //                                  //FollowUp = _followup
        //                                  _cust,
        //                                  _case1,
        //                                  _followup
        //                              };

        //        // filter the data with the conditions
        //        var _filtered_results_manual = _joined_results.Where(conditions);

        //        // there is at least one result
        //        if (_filtered_results_manual.Count() > 0)
        //        {
        //            // assign results' row values to the CustCaseDetails object
        //            foreach (var _result_item in _filtered_results_manual)
        //            {
        //                // declare CustCaseDetails object
        //                CustCaseDetails _details = new CustCaseDetails();

        //                _details.Customer_Id = _result_item._cust.Customer_Id;
        //                _details.Created_Time = _result_item._cust.Created_Time.Value;
        //                _details.Created_By = _result_item._cust.Created_By.ToString();
        //                _details.Updated_Time = _result_item._cust.Updated_Time.Value;
        //                _details.ID_No = _result_item._cust.ID_No;
        //                _details.Name_Chi = _result_item._cust.Name_Chi;
        //                _details.Name_Eng = _result_item._cust.Name_Eng;
        //                _details.Gender = _result_item._cust.Gender;
        //                _details.Title = _result_item._cust.Title;
        //                _details.Lang = _result_item._cust.Lang;
        //                _details.DOB = (_result_item._cust.DOB != null) ? _result_item._cust.DOB.Value : DateTime.MinValue;
        //                _details.Home_No = _result_item._cust.Home_No;
        //                _details.Office_No = _result_item._cust.Office_No;
        //                _details.Mobile_No = _result_item._cust.Mobile_No;
        //                _details.Fax_No = _result_item._cust.Fax_No;
        //                _details.Other_Phone_No = _result_item._cust.Other_Phone_No;
        //                _details.Email = _result_item._cust.Email;
        //                _details.Address1 = _result_item._cust.Address1;
        //                _details.Address2 = _result_item._cust.Address2;
        //                _details.Address3 = _result_item._cust.Address3;
        //                _details.Address4 = _result_item._cust.Address4;
        //                _details.Full_Address = _result_item._cust.Full_Address;
        //                _details.Occupation = _result_item._cust.Occupation;
        //                _details.Case_No = (_result_item._case1.Case_No != null) ? _result_item._case1.Case_No.Value : 0;
        //                _details.Call_Nature = _result_item._case1.Call_Nature;
        //                _details.Details = _result_item._case1.Details;
        //                _details.Status = _result_item._case1.Status;
        //                _details.Remarks = _result_item._case1.Remark;
        //                _details.Agent = _result_item._case1.Created_By.ToString();
        //                _details.Creation_Date_Time = _result_item._case1.Created_Time.Value;

        //                _all_cases.Add(_details);

        //            }
        //        }
        //        else
        //        {
        //            _all_cases = null;
        //        }
        //    }

        //    // return the CustCaseDetails List object
        //    return _all_cases;
        //}


        //// Perform manual search (old method)
        //public List<contact_list_item> manualSearch([FromBody] dynamic data)
        //{
        //    // obtain form body values
        //    var searchArray = data.searchArr;
        //    string validityType = (data.Is_Valid == null) ? "all" : data.Is_Valid.Value;

        //    // obtain data from table contact_list
        //    var _search_results_contacts = (from _contact in _scrme.contact_list
        //                                    select _contact).AsQueryable();

        //    // obtain the search conditions from getManualSearchConditions([FromBody] dynamic data)
        //    string whereClause = getManualSearchConditions(data);

        //    // filter the search results with the where clause
        //    _search_results_contacts = _search_results_contacts.Where(whereClause);

        //    // return the results in List format
        //    return _search_results_contacts.ToList();
        //}

        //public JObject getCustomerManualSearchResults([FromBody] dynamic data)
        //{
        //    // declare a list of json objects containing the each row of data
        //    List<JObject> jsonResultList = new List<JObject>();

        //    // declare a json object to contain all rows of data
        //    JObject allJsonResults = new JObject();

        //    // obtain a list of data from table "case_result"
        //    List<case_result_list_item> _linq_case = (from _v in _scrme.case_result
        //                                              select _v).ToList();

        //    // declare a list of contact_list_item data
        //    List<contact_list_item> _search_results_contacts = new List<contact_list_item>();

        //    // declare a list of contact_list_log_item data
        //    List<contact_list_log_item> _search_results_logs = new List<contact_list_log_item>();

        //    bool haveCase = false; // declare have case value

        //    // obtain required validity from form body
        //    string validityType = (data.Is_Valid == null) ? string.Empty : data.Is_Valid.Value;

        //    // get the search results
        //    _search_results_contacts = combineCustomerAndReplySearchResults(data);
        //    _search_results_logs = manualSearchInContactListLog(data);

        //    // filter the search results by validity
        //    _search_results_contacts = filterByContactValidity(_search_results_contacts, validityType);
        //    _search_results_logs = filterByLogValidity(_search_results_logs, validityType);

        //    jsonResultList = getJsonList(_search_results_contacts, _search_results_logs);

        //    allJsonResults = new JObject()
        //    {
        //            new JProperty("result", "success"),
        //            new JProperty("details", jsonResultList)
        //    };

        //    // return all results in json format
        //    return allJsonResults;
        //}

        //public JObject getCustSearchResults([FromBody] dynamic data, string searchType)
        //{
        //    // declare a list of json objects containing the each row of data
        //    List<JObject> jsonResultList = new List<JObject>();

        //    // declare a json object to contain all rows of data
        //    JObject allJsonResults = new JObject();

        //    // obtain a list of data from table "case_result"
        //    List<case_result_list_item> _linq_case = (from _v in _scrme.case_result
        //                                              select _v).ToList();

        //    // declare a list of contact_list_item data
        //    List<contact_list_item> _search_results = new List<contact_list_item>();

        //    bool haveCase = false;

        //    string validityType = (data.Is_Valid == null) ? string.Empty : data.Is_Valid.Value;

        //    // search by different types of search: auto and manual
        //    if (searchType == "auto")
        //    {
        //        // call autoSearch function
        //        _search_results = autoSearch(data);


        //    }
        //    else if (searchType == "manual")
        //    {
        //        // call manualSearch function
        //        _search_results = manualSearch(data);
        //    }

        //    // filter the search result by the required validity
        //    _search_results = filterByContactValidity(_search_results, validityType);

        //    jsonResultList = getJsonList(_search_results, null);

        //    allJsonResults = new JObject()
        //    {
        //            new JProperty("result", "success"),
        //            new JProperty("details", jsonResultList)
        //    };

        //    // return all results in json format
        //    return allJsonResults;
        //}
    }
}