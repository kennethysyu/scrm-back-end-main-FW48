using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrm1.Class
{
    public class Class1
    {

        //// determine value of replyStatements 123 456
        //if (replyType == "allCalls")
        //{
        //    // handle "All_Phone_No"
        //    string tempStatement1 = getPartialStatementsFromReply("Inbound_Call", replyDetails);
        //    string tempStatement2 = getPartialStatementsFromReply("Inbound_Fax", replyDetails);

        //    // determine reply statements based on the contents of the temp statements
        //    if (tempStatement1 == string.Empty && tempStatement2 == string.Empty)
        //    {
        //        replyStatements = string.Empty;
        //    } else if (tempStatement1 == string.Empty && tempStatement2 != string.Empty)
        //    {
        //        replyStatements = tempStatement2;
        //    } else if (tempStatement1 != string.Empty && tempStatement2 == string.Empty)
        //    {
        //        replyStatements = tempStatement1;
        //    } else
        //    {
        //        //tempStatement1 != string.Empty && tempStatement2 != string.Empty
        //        if (tempStatement1 == tempStatement2)
        //        {
        //            replyStatements = tempStatement1;
        //        } else
        //        {
        //            replyStatements = "(" + tempStatement1 + " OR " + tempStatement2 + ")";
        //        }
        //    }
        //}
        //else
        //{
        //    replyStatements = getPartialStatementsFromReply(replyType, replyDetails); // simply get reply statments
        //}

        //if (replyStatements != string.Empty)
        //{
        //    conditionList.Add(replyStatements); // add to condition list
        //}

        //// Perform manual search
        //public List<contact_list_item> manualSearch([FromBody] dynamic data)
        //{
        //    // obtain form body values
        //    string anyAll = data.anyAll.Value;
        //    var searchArray = data.searchArr;

        //    // obtain data from table contact_list
        //    var _contact_results = (from _contact in _scrme.contact_list
        //                            select _contact).AsQueryable();

        //    // declare the conditions List to store the array of conditions
        //    var conditionList = new List<string>();

        //    // declare the overall statement for the where clause
        //    var whereClause = string.Empty;

        //    // declare an int array to store the year, month and day split from the date value
        //    int[] yearMonthDay; // _year_month_day[0] = Year, ...etc

        //    // iterate through each search array item
        //    for (int i = 0; i < searchArray.Count; i++)
        //    {
        //        // declare the single statement
        //        string statement = string.Empty;

        //        // separate the search array item and assign the values into local variables
        //        string fieldName = searchArray[i].field_name.Value;
        //        string logicOperator = searchArray[i].logic_operator.Value;
        //        string fieldValue = searchArray[i].value.Value;
        //        string fieldType = (searchArray[i].field_type == null) ? string.Empty : searchArray[i].field_type.Value;

        //        // For fields of DateTime type
        //        if (fieldType == "datetime")
        //        {
        //            // split the _field_value to year, month and day and store them in the int array, 
        //            string[] ymd = fieldValue.Split('/'); // e.g. 2018 /01/ 01 to {2018, 01, 01}
        //            yearMonthDay = Array.ConvertAll(ymd, (v) => Convert.ToInt32(v));

        //            // form a single statement consisting of field name, operator and value
        //            switch (logicOperator)
        //            {
        //                // change the form body operator to a linq-readable operator
        //                case "is":
        //                case "contains":
        //                case "=":
        //                    {
        //                        logicOperator = "==";
        //                        //_statement = _field_name + _operator + "(DateTime(" + _year_month_day[0] + ", " + _year_month_day[1] + ", " + _year_month_day[2] + "))";
        //                        break;
        //                    }

        //                case "is not":
        //                case "does not contain":
        //                    {
        //                        logicOperator = "!=";
        //                        break;
        //                    }

        //                default: // For !=, >, >=, <, <=
        //                    break;
        //            }

        //            // Form the statement with the modified operator
        //            statement = fieldName + ".Value.Year == " + yearMonthDay[0] + " &&" +
        //                                      fieldName + ".Value.Month == " + yearMonthDay[1] + " &&" +
        //                                      fieldName + ".Value.Day" + logicOperator + yearMonthDay[2];
        //        }
        //        else // For other fields
        //        {
        //            if (fieldName == "All_Phone_No")
        //            {

        //            }
        //            else if (fieldName == "Email")
        //            {

        //            }
        //            else
        //            {
        //                PropertyInfo properInfo = _contact_results.First().GetType().GetProperty(fieldName);
        //                // form a single statement consisting of field name, operator and value
        //                switch (logicOperator)
        //                {
        //                    // change the form body operator to a linq-readable operator
        //                    case "is":
        //                        logicOperator = "==";
        //                        if (properInfo.PropertyType.AssemblyQualifiedName.Contains("Int"))
        //                        {
        //                            statement = fieldName + logicOperator + fieldValue; // no need "" for int value
        //                        }
        //                        else
        //                        {
        //                            statement = fieldName + logicOperator + "\"" + fieldValue + "\"";
        //                        }
        //                        break;

        //                    case "is not":
        //                        logicOperator = "!=";
        //                        if (properInfo.PropertyType.AssemblyQualifiedName.Contains("Int"))
        //                        {
        //                            statement = fieldName + logicOperator + fieldValue; // no need "" for int value
        //                        }
        //                        else
        //                        {
        //                            statement = fieldName + logicOperator + "\"" + fieldValue + "\"";
        //                        }
        //                        break;

        //                    case "contains":
        //                        logicOperator = ".Contains";
        //                        statement = fieldName + logicOperator + "(\"" + fieldValue + "\")";
        //                        break;

        //                    case "does not contain":
        //                        logicOperator = ".Contains";
        //                        statement = "!" + fieldName + logicOperator + "(\"" + fieldValue + "\")";
        //                        break;

        //                    case "=":
        //                        logicOperator = "==";
        //                        statement = fieldName + logicOperator + fieldValue;
        //                        break;

        //                    default: // for >, <, >= and <=
        //                             // the value of logicOperator does not need to be changed
        //                        statement = fieldName + logicOperator + fieldValue;
        //                        break;
        //                }
        //            }
        //        }

        //        // add the statement to the conditions List
        //        conditionList.Add(statement);
        //    }

        //    if (anyAll == "any")
        //    {
        //        // join the conditions with "OR"
        //        whereClause = string.Join(" OR ", conditionList.ToArray());
        //    }
        //    else // all
        //    {
        //        // join the conditions with "AND"
        //        whereClause = string.Join(" AND ", conditionList.ToArray());
        //    }

        //    // retrieve the data using the content in the where clause
        //    _contact_results = _contact_results.Where(whereClause);

        //    // return the results in List format
        //    return _contact_results.ToList();
        //}

        //int companyId = 0;
        //if (value.companyID != null) companyId = Convert.ToInt32(value.companyID.Value);
        //string templateName = "";
        //if (value.templateName != null) templateName = value.templateName.Value;
        //int formId = 0;
        //if (value.formId != null) formId = Convert.ToInt32(value.formId.Value);

        //if (companyId <= 0) return Json(new { result = "error", message = "Invalid Company Id." });
        //if (templateName == "") return Json(new { result = "error", message = "Invalid Template Name." });
        //if (formId <= 0) return Json(new { result = "error", message = "Invalid Form Id." });

        //var _result_by_id = from _cust in _scrme.contact_list
        //                    where _cust.Customer_Id == customer_id
        //                    join _case in _scrme.case_result
        //                        on _cust.Customer_Id equals _case.Customer_Id
        //                        into _case_group
        //                    select new
        //                    {
        //                        _cust,
        //                        _cases = from _item in _case_group
        //                                 orderby _item.Customer_Id
        //                                 select _item
        //                    };


        //var _result_by_id = from _cust in _scrme.contact_list
        //                    where _cust.Customer_Id == customer_id
        //                    join _case in _scrme.case_result
        //                        on _cust.Customer_Id equals _case.Customer_Id
        //                        into _case_group
        //                    select new
        //                    {
        //                        _cust.Address1,
        //                        _cust.Customer_Id,
        //                        _case_group.
        //                    };

        //foreach (string _item in _scrm_functions.contactToString(_search_result_by_details))
        //{
        //    var _case = _linq_case.Where(_c => _c.Customer_Id == _item.);

        //    // there's at least a case for that particular customer id
        //    if (_case.Count() > 0)
        //    {
        //        _have_case = true;

        //    }


        //}



        //var _contact_result = new
        //{
        //    have_case = _have_case.ToString(),
        //    address = _contact_item.Address2
        //};

        //// find out the cases of a particular customer id
        //var _case = _linq_case.Where(_c => _c.Customer_Id == _contact_item.Customer_Id);

        //// there's at least a case for that particular customer id
        //if (_case.Count() > 0)
        //{
        //    _have_case = true;
        //    _search_result.Add(_contact_item.ToString());
        //}

        //// add the cases to the List
        //_cust_case.AddRange(_case);
        //_contact_detail.Add(_contact_item.ToString());
        // _search_result.Add(_contact_result);





        //List<string> _contact_detail = new List<string>();



        //_search_result = _result.ToList();

        //_contact_detail.Add(_result);

        //// assign _have_case values
        //if (_cust_case.Count() > 0)
        //    {
        //        _have_case = true;
        //    }

        //    return Json(new
        //    {
        //        result = "success",
        //        have_case = _have_case,
        //        //test = _contact_detail,
        //        details = _result
        //    });
        //}


        //var _cust_by_id =   from _cust in _scrme.contact_list
        //                    where _cust.Customer_Id == customer_id
        //                    select _cust;

        //var _case_by_id = from _case in _scrme.case_result
        //                  where _case.Customer_Id == customer_id
        //                  select _case;

        //// declare a list of customer ids
        //List<int> _cust_id_list = new List<int>();

        //// obtain a list of customer ids from the contact_list search result
        //foreach (var _result_item in _cust_by_id)
        //{
        //    _cust_id_list.Add(_result_item.Customer_Id);
        //}

        ////var _case_by_id = from _case in _scrme.case_result
        ////                  where _cust_id_list.Contains((int)_case.Customer_Id)
        ////                  select _case;

        //IQueryable<contact_list_item> test2 = _cust_by_id.Concat(_case_by_id);

        //var _cust_and_case = Enumerable.Union(_cust_by_id, _case_by_id);

        //var a = Json(new { result = "success" });


        //// use the obtained customer ids to search for cases
        //var _result_by_group = from _cust in _scrme.contact_list
        //                       where _cust_id_list.Contains(_cust.Customer_Id)
        //                       join _case in _scrme.case_result
        //                           on _cust.Customer_Id equals _case.Customer_Id
        //                           into _case_group
        //                       from _case in _case_group.DefaultIfEmpty()
        //                       select new
        //                       {
        //                           _cust,
        //                           _case
        //                       };



        //else
        //{
        //    if (details != null)
        //    {
        //        // call getCustSearchResult function to get customers based on the search criteria
        //        var _search_result_by_details = _scrm_functions.getCustByDetails(channel, details);

        //        List<string> _search_result = new List<string>();

        //        // output results and find respective cases
        //        if (_search_result_by_details.Count() > 0)
        //        {
        //            // declare a boolean variable _have_case
        //            bool _have_case = false;

        //            // declare a list of customer cases
        //            List<case_result_list_item> _cust_case = new List<case_result_list_item>();

        //            List<ResultDetails> _results = new List<ResultDetails>();

        //            // iterate through the customers
        //            foreach (contact_list_item _contact_item in _search_result_by_details)
        //            {
        //                ResultDetails _result_item = new ResultDetails();

        //                var _case = _linq_case.Where(_c => _c.Customer_Id == _contact_item.Customer_Id);

        //                // there's at least a case for that particular customer id
        //                if (_case.Count() > 0)
        //                {
        //                    _have_case = true;

        //                }


        //                _result_item.have_case = _have_case;
        //                _result_item.customer_info = _contact_item;

        //                _results.Add(_result_item);
        //            }

        //            return Json(new
        //            {
        //                result = "success",
        //                details = _results,
        //                test = _search_result
        //                //details = _search_result_by_details,
        //                //details2 = _search_result
        //            });
        //        }

        //        else
        //        {
        //            return Json(new { result = "fail", details = "no records" });
        //        }
        //    }
        //    else
        //    {
        //        // give all data in table contact_list
        //        return Json(new { result = "success", details = _linq });
        //    }

        //}

        //// obtain form body values (either channel+details or just customer_id)
        //string channel = (data.channel == null) ? "x" : data.channel.Value;
        //string details = (data.details == null) ? "x" : data.details.Value;
        //int customer_id = (data.Customer_id == null) ? -1 : (int)data.Customer_id.Value;

        //// declare search condition and parameter
        ////string condition = "@0.Contains(outerIt._cust.Customer_Id)";
        //List<int> parameter = new List<int>();

        //// declare CustCaseDetails list
        //List<CustCaseDetails> _search_result_details = new List<CustCaseDetails>();

        //// if customer id is given
        //if (customer_id != -1)
        //{
        //    // assign the value of parameter
        //    parameter.Add(customer_id);

        //    // run the function of join tables and get the list of results
        //    _search_result_details = _scrm_functions.joinTables(data, parameter);
        //} else
        //{
        //    // call getCustSearchResult function to get customers based on the search criteria
        //    var _search_result_by_details = _scrm_functions.getCustByDetails(channel,details);

        //    // declare a list of customer ids
        //    List<int> _cust_id_list = new List<int>();

        //    // obtain a list of customer ids from the contact_list search result
        //    foreach (var _result_item in _search_result_by_details)
        //    {
        //        _cust_id_list.Add(_result_item.Customer_Id);
        //    }

        //    // assign the values of parameter
        //    parameter = _cust_id_list;

        //    // run the function of join tables and get the list of results
        //    _search_result_details = _scrm_functions.joinTables(data, parameter);
        //}


        //// declare new entity object
        //scrmEntities _scrme = new scrmEntities();

        //// obtain a list of data from table "case_result"
        //List<case_result_list_item> _linq_case = (from _v in _scrme.case_result
        //                                          select _v).ToList();

        //// obtain form body values
        //string _any_all = data.anyAll.Value;
        //var _search_arr = data.searchArr;

        //// obtain data from table contact_list
        //var _contact_result = (from _contact in _scrme.contact_list
        //                       select _contact).AsQueryable();

        //// declare the conditions List to store the array of conditions
        //var _conditions = new List<string>();

        //// declare the overall statement for the where clause
        //var _where_clause = "";

        //// iterate through each search array item
        //for (int i = 0; i < _search_arr.Count; i++)
        //{
        //    // declare the single statement
        //    string _statement = "";

        //    // separate the search array item and assign the values into local variables
        //    string _field_name = _search_arr[i].field_name.Value;
        //    string _operator = _search_arr[i].logic_operator.Value;
        //    string _field_value = _search_arr[i].value.Value;

        //    // form a single statement consisting of field name, operator and value
        //    switch (_operator)
        //    {
        //        // change the form body operator to a linq-readable operator
        //        case "is":
        //            _operator = "==";
        //            _statement = _field_name + _operator + "\"" + _field_value + "\"";
        //            break;

        //        case "is not":
        //            _operator = "!=";
        //            _statement = _field_name + _operator + "\"" + _field_value + "\"";
        //            break;
        //        case "contains":
        //            _operator = ".Contains";
        //            _statement = _field_name + _operator + "(\"" + _field_value + "\")";
        //            break;
        //        case "does not contain":
        //            _operator = "";
        //            _statement = "!" + _field_name + _operator + "(\"" + _field_value + "\")";
        //            break;
        //        case "=":
        //            _operator = "==";
        //            _statement = _field_name + _operator + _field_value;
        //            break;
        //        default:
        //            // the value of _operator does not need to be changed
        //            _statement = _field_name + _operator + _field_value;
        //            break;

        //    }

        //    // add the statement to the conditions List
        //    _conditions.Add(_statement);
        //}

        //if (_any_all == "any")
        //{
        //    // join the conditions with "OR"
        //    _where_clause = string.Join(" OR ", _conditions.ToArray());
        //} else // all
        //{
        //    // join the conditions with "AND"
        //    _where_clause = string.Join(" AND ", _conditions.ToArray());
        //}

        //// retrieve the data using the content in the where clause
        //_contact_result = _contact_result.Where(_where_clause);

        //// return result
        //if (_contact_result.Count() > 0)
        //{
        //    // declare a boolean variable _have_case
        //    bool _have_case = false;

        //    // declare a list of customer cases
        //    List<case_result_list_item> _cust_case = new List<case_result_list_item>();

        //    // iterate through the customers
        //    foreach (contact_list_item _contact_item in _contact_result)
        //    {
        //        // find out the cases of a particular customer id
        //        var _case = _linq_case.Where(_c => _c.Customer_Id == _contact_item.Customer_Id);

        //        // add the cases to the List
        //        _cust_case.AddRange(_case);
        //    }

        //    // assign _have_case values
        //    if (_cust_case.Count() > 0)
        //    {
        //        _have_case = true;
        //    }

        //    return Json(new
        //    {
        //        result = "success",
        //        have_case = _have_case,
        //        details = _contact_result
        //    });

        //} else
        //{
        //    return Json(new
        //    {
        //        result = "fail",
        //        details = "no records"
        //    });
        //}

        //// return result
        //if (_contact_result.Count() > 0)
        //{
        //    // declare a boolean variable _have_case
        //    bool _have_case = false;

        //    // declare a list of customer cases
        //    List<case_result_list_item> _cust_case = new List<case_result_list_item>();

        //    // iterate through the customers
        //    foreach (contact_list_item _contact_item in _contact_result)
        //    {
        //        // find out the cases of a particular customer id
        //        var _case = _linq_case.Where(_c => _c.Customer_Id == _contact_item.Customer_Id);

        //        // add the cases to the List
        //        _cust_case.AddRange(_case);
        //    }

        //    // assign _have_case values
        //    if (_cust_case.Count() > 0)
        //    {
        //        _have_case = true;
        //    }

        //    return Json(new
        //    {
        //        result = "success",
        //        have_case = _have_case,
        //        details = _contact_result
        //    });

        //}
        //else
        //{
        //    return Json(new
        //    {
        //        result = "fail",
        //        details = "no records"
        //    });
        //}

        //// find respective cases in each search result item
        //if (_search_result_by_details.Count() > 0)
        //{
        //    // iterate through the customers
        //    foreach (contact_list_item _contact_item in _search_result_by_details)
        //    {
        //        CustomerDetails _cust_item = new CustomerDetails();

        //        // retrieve the cases based on the searched customer ids
        //        var _case = _linq_case.Where(_c => _c.Customer_Id == _contact_item.Customer_Id);

        //        // there's at least a case for that particular customer id
        //        if (_case.Count() > 0)
        //        {
        //            // assign true to "have_case"
        //            _cust_item.have_case = true;
        //        }
        //        else
        //        {
        //            _cust_item.have_case = false;
        //        }

        //        // assign the table data to the CustomerDetails class
        //        _cust_item.Customer_Id = _contact_item.Customer_Id;
        //        _cust_item.Created_Time = _contact_item.Created_Time.Value;
        //        _cust_item.Created_By = _contact_item.Created_By.ToString();
        //        _cust_item.Updated_Time = _contact_item.Updated_Time.Value;
        //        _cust_item.ID_No = _contact_item.ID_No;
        //        _cust_item.Name_Chi = _contact_item.Name_Chi;
        //        _cust_item.Name_Eng = _contact_item.Name_Eng;
        //        _cust_item.Gender = _contact_item.Gender;
        //        _cust_item.Title = _contact_item.Title;
        //        _cust_item.Lang = _contact_item.Lang;
        //        _cust_item.DOB = (_contact_item.DOB != null) ? _contact_item.DOB.Value : DateTime.MinValue;
        //        _cust_item.Home_No = _contact_item.Home_No;
        //        _cust_item.Office_No = _contact_item.Office_No;
        //        _cust_item.Mobile_No = _contact_item.Mobile_No;
        //        _cust_item.Fax_No = _contact_item.Fax_No;
        //        _cust_item.Other_Phone_No = _contact_item.Other_Phone_No;
        //        _cust_item.Email = _contact_item.Email;
        //        _cust_item.Address1 = _contact_item.Address1;
        //        _cust_item.Address2 = _contact_item.Address2;
        //        _cust_item.Address3 = _contact_item.Address3;
        //        _cust_item.Address4 = _contact_item.Address4;
        //        _cust_item.Full_Address = _contact_item.Full_Address;
        //        _cust_item.Occupation = _contact_item.Occupation;

        //        // append to list
        //        _cust_details.Add(_cust_item);
        //    }
        //}

        //else
        //{
        //    _cust_details = null;
        //}

        //return _cust_details;

        //// declare new entity object
        //scrmEntities _scrme = new scrmEntities();

        ////declare db table items
        //case_result_list_item _case_item = new case_result_list_item();
        //new_case_no_list_item _new_case_no_item = new new_case_no_list_item();

        //// obtain a list of data from table "case_result"
        //List<case_result_list_item> _linq = (from _v in _scrme.case_result
        //                                     select _v).ToList();

        //// obtain form body values
        //int internal_case_no = (int)data.Internal_Case_No.Value;
        //int agent_id = (int)data.Agent_Id.Value;
        //string call_nature = data.Call_Nature.Value;
        //string details = data.Details.Value;
        //string remarks = data.Remark.Value;
        //string status = data.Status.Value;

        //// see if internal case no already exists
        //var _case = _linq.Where(_c => _c.Internal_Case_No == internal_case_no);

        //// customer exists in table
        //if (_case.Count() > 0)
        //{
        //    // add new case no
        //    _new_case_no_item.Time_Stamp = DateTime.Now;
        //    _scrme.new_case_no.Add(_new_case_no_item);
        //    _scrme.SaveChanges();

        //    // retrieve the row to be updated
        //    _case_item = _scrme.case_result.Where(c => c.Internal_Case_No == internal_case_no).Single<case_result_list_item>();

        //    // assign the updated values to the row
        //    _case_item.Case_No = _new_case_no_item.Case_No; // assign the newly added Case_No from new_case_no table
        //    _case_item.Internal_Case_No = internal_case_no;
        //    _case_item.Updated_By = agent_id;
        //    _case_item.Updated_Time = DateTime.Now;
        //    _case_item.Call_Nature = call_nature;
        //    _case_item.Details = details;
        //    _case_item.Status = status;
        //    _case_item.Remark = remarks;
        //    _case_item.Is_Valid = "Y";

        //    // update status and save changes in db
        //    _scrme.Entry(_case_item).State = System.Data.Entity.EntityState.Modified;
        //    _scrme.SaveChanges();

        //    // return successful update
        //    return Json(new { result = "success", details = "updated case." });

        //// case does not exist
        //}
        //else
        //{
        //    // return unsuccessful update
        //    return Json(new { result = "fail", details = "case does not exist" });
        //}
    }
}