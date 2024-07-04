using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.Http;
using System.Net.Http;
using System.Text;
using scrm1.Models;
using scrm1.Class;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace scrm1.Controllers
{
    public interface scrmOtherFunctions
    {
        JObject login([FromBody] dynamic data);
        JObject loginldap([FromBody] dynamic data);
        JObject getLogin();
        List<agent_info_list_item> createUser([FromBody] dynamic data);
        bool userExists([FromBody] dynamic data);
        bool sellerIdExists([FromBody] dynamic data);
        string getAgentsOfRole([FromBody] dynamic data);
        //void saveAgentPhoto(int agentId, byte[] photo, string fileType);
        string saveAgentPhoto(int agentId, byte[] photo, string fileType);
        JObject getAgentPhoto([FromBody] dynamic data);
        void updateUser([FromBody] dynamic data);
        string changePassword([FromBody] dynamic data);
        //JObject getPhoto([FromBody] dynamic data);
        Dictionary<string, List<FieldDetails>> getFields([FromBody] dynamic data);
        List<user_role_list_item> createRole([FromBody] dynamic data);
        JObject getRoles(string status);
        void updateRole([FromBody] dynamic data);

        void addScheduleSetting([FromBody] dynamic data);
        void updateScheduleSetting([FromBody] dynamic data);
        List<task_schedule_setting_list_item> getScheduleSetting();
        List<task_schedule_record_list_item> checkScheduleAlert();
        void handleScheduleAlert([FromBody] dynamic data);
        List<task_schedule_record_list_item> getScheduleHistory(string stype);

        JObject getFloorPlan(string ftype, int fid);
        void addFloorPlan([FromBody] dynamic data);
        void updateFloorPlan([FromBody] dynamic data);

        bool authenticated(string token, string P_Username);

    }

    public interface scrmFacebookFunctions
    {
        List<facebook_post_list_item> createFacebookPostContent([FromBody] dynamic data);
        void saveFbMedia(int ticketId, int agentId, string mediaType, string mediaLink);
        bool ticketIdExists([FromBody] dynamic data);
        JObject getFacebookPostContent(int ticketId);
        void updateFacebookPostContent([FromBody] dynamic data);
    }

    [EnableCors("*", "*", "*")]
    
    public class FieldDetails
    {
        public string Field_Name;
        public string Field_Display;
        public string Field_Tag;
        public string Field_Type;
        public List<string> Field_Options;
    }

    public class ConfigDetails
    {
        public int P_Id;
        public string P_Name;
        public string P_Value;
    }

    public class mainController : ApiController
    {
        private scrmEntities _scrme = new scrmEntities();

        //declare new interface class object
        private scrmOtherFunctions _scrm_other_functions = new scrm_others();
        private scrmFacebookFunctions _scrm_facebook_functions = new scrm_facebook();

        //===============================1: Login======================================
        // declare route for auto search
        [Route("api/Login/")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult Login([FromBody] dynamic data)
        {
            string is_ldap = (data.Is_Ldap == null) ? string.Empty : Convert.ToString(data.Is_Ldap.Value);

            try
            {
                if (is_ldap.ToUpper() == "Y")
                {
                    return Json(_scrm_other_functions.loginldap(data));
                }
                else
                {
                    return Json(_scrm_other_functions.login(data));
                }
            }
            catch (Exception err)
            {

                //return Json(new { result = "fail", details = err.Message });
                return Json(new { result = "fail", details = "Invalid Parameters" });

            }
        }

        //===============================: GetLogin======================================
        // declare route for auto search
        [Route("api/GetLogin/")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult GetLogin([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    // get a list of agents
                    return Json(_scrm_other_functions.getLogin());
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //===============================: CreateUser======================================
        // declare route for auto search
        [Route("api/CreateUser/")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult CreateUser([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    // get a list of agents
                    return Json(new { result = "success", details = _scrm_other_functions.createUser(data) });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                //return Json(new { result = "fail", details = err.Message });
                return Json(new { result = "fail", details = "cannot create user." });
            }
        }

        //===============================: CheckAgentId======================================
        // declare route for auto search
        [Route("api/CheckAgentId")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult CheckAgentId([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    bool userExists = _scrm_other_functions.userExists(data);
                    return Json(new { result = userExists });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            } catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });
            }
            
        }

        //===============================: CheckSellerId======================================
        // declare route for auto search
        [Route("api/CheckSellerId")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult CheckSellerId([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    //  bool sellerExists = _scrm_other_functions.userExists(data);
                    bool sellerExists = _scrm_other_functions.sellerIdExists(data);
                    return Json(new { result = sellerExists });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });
            }
        }

        //===============================: GetAgentsOfRole======================================
        // declare route for auto search
        [Route("api/GetAgentsOfRole")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult GetAgentsOfRole([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    string agentsOfRole = _scrm_other_functions.getAgentsOfRole(data);
                    return Json(new { result = "success", details = agentsOfRole });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });
            }
        }

        //=======================
        // declare route for getting photos
        //[Route("api/GetAgentPhoto/")]

        //// declare HTTP Post request
        //[HttpPost]

        //public IHttpActionResult GetAgentPhoto([FromBody] dynamic data)
        //{
        //    string token = (data.Token == null) ? string.Empty : data.Token.Value;
        //    string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

        //    try
        //    {
        //        if (_scrm_other_functions.authenticated(token, tk_agentId))
        //        {
        //            return Json(_scrm_other_functions.getAgentPhoto(data));
        //        }
        //        else
        //        {
        //            return Json(new { result = "fail", details = "Not Auth." });
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        return Json(new { result = "fail", details = err.Message });

        //    }
        //}

        //===============================: UpdateUser======================================
        // declare route for auto search
        [Route("api/UpdateUser/")]

        // declare HTTP request
        [HttpPut]

        public IHttpActionResult UpdateUser([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    _scrm_other_functions.updateUser(data);
                    // get results
                    return Json(new { result = "success", details = "updated user" });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });
                // return Json(new { result = "fail", details = "cannot update user" });

            }
        }

        //===============================: ChangePassword======================================
        // declare route for auto search
        [Route("api/ChangePassword/")]

        // declare HTTP request
        [HttpPut]

        public IHttpActionResult ChangePassword([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    // get results
                    return Json(new { result = "success", details = _scrm_other_functions.changePassword(data)});
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                //return Json(new { result = "fail", details = err.Message });
                return Json(new { result = "fail", details = "cannot change password" });
            }
        }

        //===============================4: GetFields======================================
        // declare route for getting fields
        [Route("api/GetFields/")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult GetFields([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    // declare a dictionary object where key = Field_Category, value = FieldDetails's values
                    Dictionary<string, List<FieldDetails>> tableFields = new Dictionary<string, List<FieldDetails>>();

                    // obtain the fields of the given list
                    tableFields = _scrm_other_functions.getFields(data);

                    // return result
                    return Json(new
                    {
                        result = "success",
                        details = tableFields
                    });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                //return Json(new { result = "fail", details = err.Message });
                return Json(new { result = "fail", details = "invalid parameters" });
            }
        }

        ////=======================GetPhoto
        //// declare route for getting photos
        //[Route("api/GetPhoto/")]

        //// declare HTTP Post request
        //[HttpPost]

        //public IHttpActionResult GetPhoto([FromBody] dynamic data)
        //{
        //    try
        //    {
        //        return Json(_scrm_other_functions.getPhoto(data));
        //    }
        //    catch (Exception err)
        //    {
        //        return Json(new { result = "fail", details = err.Message });

        //    }
        //}

        //=======================CreateRole
        [Route("api/CreateRole/")]

        // declare HTTP Post request
        [HttpPost]

        public IHttpActionResult CreateRole([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    // create new case
                    List<user_role_list_item> _new_role = _scrm_other_functions.createRole(data);

                    return Json(new
                    {
                        result = "success",
                        details = _new_role
                    });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //=======================GetRoles
 //       [Route("api/GetRoles/{status}")]
        [Route("api/GetRoles/")]

        // declare HTTP Post request
        [HttpPost]

  //      public IHttpActionResult GetRoles(string status)
        public IHttpActionResult GetRoles([FromBody] dynamic data)
        {

            string status = (data.RoleStatus == null) ? string.Empty : data.RoleStatus.Value;

            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);
            
            try
            {
                if ( _scrm_other_functions.authenticated(token, tk_agentId) )
                {
                    return Json(_scrm_other_functions.getRoles(status));
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
                                
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //=======================UpdateRole
        [Route("api/UpdateRole/")]

        // declare HTTP Post request
        [HttpPut]

        public IHttpActionResult UpdateRole([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    _scrm_other_functions.updateRole(data);
                    // get results
                    return Json(new { result = "success", details = "updated user role" });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //==============================9. UploadPhoto==========================
        // declare route for uploading photos
        [Route("api/UploadPhoto/")]

        // declare HTTP Post request
        [HttpPost]

        public IHttpActionResult UploadPhoto()
        {
            try
            {
                string token = string.Empty;
                string tk_agentId = string.Empty;

                // declare httpcontext and get file
                var httpContext = (HttpContextWrapper)Request.Properties["MS_HttpContext"];
                HttpPostedFileBase file = httpContext.Request.Files[0];

                // interate through form body to obtain customer id
             //   int customerId = 0;
                int agentId = 0;
                for (int i = 0; i < httpContext.Request.Form.Keys.Count; i++)
                {
                    //if (httpContext.Request.Form.Keys[i] == "Customer_Id")
                    //{
                    //    customerId = Convert.ToInt32(httpContext.Request.Form[i]);
                    //}

                    if (httpContext.Request.Form.Keys[i] == "To_Change_Id")
                    {
                        agentId = Convert.ToInt32(httpContext.Request.Form[i]);
                    }


                    if (httpContext.Request.Form.Keys[i] == "Agent_Id")
                    {
                   //     agentId = Convert.ToInt32(httpContext.Request.Form[i]);

                        tk_agentId = Convert.ToString(httpContext.Request.Form[i]);
                    }

                    if (httpContext.Request.Form.Keys[i] == "Token")
                    {
                        token = httpContext.Request.Form[i];
                    }
                }

                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    //                if (customerId == 0 || agentId == 0) // cannot obtain customer id nor agent id
                    if (agentId == 0) // cannot obtain agent id
                    {
                        return Json(new { result = "fail", details = "Invalid Parameters." });
                    }
                    else
                    {
                        byte[] photo = new byte[file.ContentLength];
                        file.InputStream.Read(photo, 0, file.ContentLength);  // read photo file
                        string photoType = file.ContentType;

                        // save photo and obtain the save status
                        string saveStatus = _scrm_other_functions.saveAgentPhoto(agentId, photo, photoType);

                        if (saveStatus == "success")
                        {
                            return Json(new { result = "success" });
                        }
                        else
                        {
                            return Json(new { result = "fail", details = "No such record." });
                        }
                    }
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }

            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //=======================CreateFacebookPostContent
        [Route("api/CreateFacebookPostContent/")]

        // declare HTTP Post request
        [HttpPost]

        public IHttpActionResult CreateFacebookPostContent([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    // create new facebook post
                    List<facebook_post_list_item> _new_post = _scrm_facebook_functions.createFacebookPostContent(data);

                    return Json(new
                    {
                        result = "success",
                        details = _new_post
                    });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //========================================================
        [Route("api/UploadFacebookMedia/")]

        // declare HTTP Post request
        [HttpPost]

        public IHttpActionResult UploadFacebookMedia()
        {
            try
            {
                string token = string.Empty;
                string tk_agentId = string.Empty;

                // declare httpcontext and get file
                HttpContextWrapper httpContext = (HttpContextWrapper)Request.Properties["MS_HttpContext"];
                HttpPostedFileBase file = httpContext.Request.Files[0];

                // interate through form body to obtain the ticket id
                int ticketId = 0;
                int agentId = 0;
                //string campaign = string.Empty;
                for (int i = 0; i < httpContext.Request.Form.Keys.Count; i++)
                {
                    if (httpContext.Request.Form.Keys[i] == "Ticket_Id")
                    {
                        ticketId = Convert.ToInt32(httpContext.Request.Form[i]);
                    }

                    if (httpContext.Request.Form.Keys[i] == "Agent_Id")
                    {
                        agentId = Convert.ToInt32(httpContext.Request.Form[i]);

                        tk_agentId = Convert.ToString(httpContext.Request.Form[i]);
                    }

                    if (httpContext.Request.Form.Keys[i] == "Token")
                    {
                        token = httpContext.Request.Form[i];
                    }

                    //if (httpContext.Request.Form.Keys[i] == "Campaign")
                    //{
                    //    campaign = httpContext.Request.Form[i];
                    //}
                }

                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    string mediaType = file.ContentType;

                    if (ticketId == 0) // cannot obtain ticket id
                    {
                        return Json(new { result = "fail", details = "Invalid Parameters." });
                    } else {
                        // extract only the filename
                        string fileName = Path.GetFileName(file.FileName);

                        // store the file inside the ~/App_Data/uploads folder
                        // var folder = "~/App_Data"
                        //var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(folder), fileName);

                        string rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["Facebook_Media_path"];

                        //// define branch depending on the campaign
                        //string branch = string.Empty;
                        //switch (campaign)
                        //{
                        //    case "NP360":
                        //        branch = "NP360";
                        //        break;

                        //    case "OneCallFix":
                        //        branch = "OneCallFix";
                        //        break;

                        //}

                        // decide the whole file path
                        string filePath = Path.Combine(rootPath, fileName);
                        file.SaveAs(filePath); // save to server

                        string mediaLink = ".\\fb_media\\" + fileName;

                        _scrm_facebook_functions.saveFbMedia(ticketId, agentId, mediaType, mediaLink);

                        return Json(new { result = "success",
                                          details  =new { Media_Type = mediaType,
                                                          Media_Link = mediaLink  }
                                        });
                    }

                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }


            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //===============================: CheckTicketId======================================
        // declare route for auto search
        [Route("api/CheckTicketId")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult CheckTicketId([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    bool postExists = _scrm_facebook_functions.ticketIdExists(data);
                    return Json(new { result = postExists });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });
            }

        }

        //=======================GetFaceBookPostContent
        // declare route for getting facebook's post content
        [Route("api/GetFaceBookPostContent/")]

        // declare HTTP Post request
        [HttpPost]

        public IHttpActionResult GetFacebookPostContent([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                //// declare httpcontext and ob
                //var httpContext = (HttpContextWrapper)Request.Properties["MS_HttpContext"];

                //// get from url query
                //int ticketId = Convert.ToInt32(httpContext.Request.QueryString["Ticket_Id"]);

                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    int ticketId = (data.Ticket_Id == null) ? -1 : Convert.ToInt32(data.Ticket_Id.Value);

                    // get results
                    return Json(_scrm_facebook_functions.getFacebookPostContent(ticketId));
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }

            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //=======================UpdateFacebookPostContent
        [Route("api/UpdateFacebookPostContent/")]

        // declare HTTP Post request
        [HttpPut]

        public IHttpActionResult UpdateFacebookPostContent([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    _scrm_facebook_functions.updateFacebookPostContent(data);
                    // get results
                    return Json(new { result = "success", details = "updated facebook post" });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }

            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }


        //=======================AddScheduleSetting
        [Route("api/AddScheduleSetting/")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult AddScheduleSetting([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    _scrm_other_functions.addScheduleSetting(data);
                    // get results
                    return Json(new { result = "success", details = "inserted" });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //=======================UpdateScheduleSetting
        [Route("api/UpdateScheduleSetting/")]

        // declare HTTP request
        [HttpPut]

        public IHttpActionResult UpdateScheduleSetting([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    _scrm_other_functions.updateScheduleSetting(data);
                    // get results
                    return Json(new { result = "success", details = "updated" });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }

        //==============================GetScheduleSetting==========================
        // declare route
        [Route("api/GetScheduleSetting/")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult GetScheduleSetting([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    List<task_schedule_setting_list_item> _list_cont = _scrm_other_functions.getScheduleSetting();

                    if (_list_cont != null)
                    {
                        // return successful get and display the list of data
                        return Json(new { result = "success", details = _list_cont });
                    }
                    else
                    {
                        // return unsuccessful get
                        return Json(new { result = "fail", details = "does not exist" });
                    }
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }

            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });
            }
        }

        //==============================CheckScheduleAlert==========================
        // declare route
        [Route("api/CheckScheduleAlert/")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult CheckScheduleAlert([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    List<task_schedule_record_list_item> _list_cont = _scrm_other_functions.checkScheduleAlert();

                    if (_list_cont != null)
                    {
                        // return successful get and display the list of data
                        return Json(new { result = "success", details = _list_cont });
                    }
                    else
                    {
                        // return unsuccessful get
                        return Json(new { result = "fail", details = "does not exist" });
                    }
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });
            }
        }

        //=======================HandleScheduleAlert
        [Route("api/HandleScheduleAlert/")]

        // declare HTTP request
        [HttpPut]

        public IHttpActionResult HandleScheduleAlert([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    _scrm_other_functions.handleScheduleAlert(data);
                    // get results
                    return Json(new { result = "success", details = "updated" });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }
        
        //==============================GetScheduleHistory==========================
        // declare route
        [Route("api/GetScheduleHistory/")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult GetScheduleHistory([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    string searchtype = (data.Search_Type == null) ? string.Empty : data.Search_Type.Value;

                    if (searchtype == "1month" || searchtype == "1year")
                    {
                        List<task_schedule_record_list_item> _list_cont = _scrm_other_functions.getScheduleHistory(searchtype);

                        if (_list_cont != null)
                        {
                            // return successful get and display the list of data
                            return Json(new { result = "success", details = _list_cont });
                        }
                        else
                        {
                            // return unsuccessful get
                            return Json(new { result = "fail", details = "does not exist" });
                        }

                    }
                    else
                    {
                        // wrong data
                        return Json(new { result = "fail", details = "Invalid Parameters." });
                    }
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });
            }
        }


        //=======================GetFloorPlan
        [Route("api/GetFloorPlan/")]

        // declare HTTP Post request
        [HttpPost]

        public IHttpActionResult GetFloorPlan([FromBody] dynamic data)
        {

            string ftype = (data.F_Type == null) ? string.Empty : data.F_Type.Value;
            int fid = (data.F_Id == null) ? -1 : (int)data.F_Id.Value;

            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    return Json(_scrm_other_functions.getFloorPlan(ftype, fid));
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }

            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }


        //=======================AddFloorPlan
        [Route("api/AddFloorPlan/")]

        // declare HTTP request
        [HttpPost]

        public IHttpActionResult AddFloorPlan([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    _scrm_other_functions.addFloorPlan(data);
                    // get results
                    return Json(new { result = "success", details = "inserted" });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }


        //=======================UpdateFloorPlan
        [Route("api/UpdateFloorPlan/")]

        // declare HTTP request
        [HttpPut]

        public IHttpActionResult UpdateFloorPlan([FromBody] dynamic data)
        {
            string token = (data.Token == null) ? string.Empty : data.Token.Value;
            string tk_agentId = (data.Agent_Id == null) ? string.Empty : Convert.ToString(data.Agent_Id.Value);

            try
            {
                if (_scrm_other_functions.authenticated(token, tk_agentId))
                {
                    _scrm_other_functions.updateFloorPlan(data);
                    // get results
                    return Json(new { result = "success", details = "updated" });
                }
                else
                {
                    return Json(new { result = "fail", details = "Not Auth." });
                }
            }
            catch (Exception err)
            {
                return Json(new { result = "fail", details = err.Message });

            }
        }



        ////=======================Test
        //[Route("api/RestoreNP360/")]

        //// declare HTTP Post request
        //[HttpGet]

        //public IHttpActionResult RestoreNP360([FromBody] dynamic data)
        //{
        //    try
        //    {
        //        // get results
        //        return Json(new { result = "success", details = _scrme.Restore_NP360() });
        //    }
        //    catch (Exception err)
        //    {
        //        return Json(new { result = "fail", details = err.Message });

        //    }
        //}

        ////=======================RestoreHKTB
        //[Route("api/RestoreHKTB/")]

        //// declare HTTP Post request
        //[HttpGet]

        //public IHttpActionResult RestoreHKTB([FromBody] dynamic data)
        //{
        //    try
        //    {
        //        // get results
        //        return Json(new { result = "success", details = _scrme.Restore_HKTB() });
        //    }
        //    catch (Exception err)
        //    {
        //        return Json(new { result = "fail", details = err.Message });

        //    }
        //}

        ////=======================Test
        //[Route("api/Test/")]

        //// declare HTTP Post request
        //[HttpGet]

        //public IHttpActionResult Test([FromBody] dynamic data)
        //{
        //    try
        //    {
        //        // obtain single customer record based on the customer id
        //        var _customer = (from _c in _scrme.contact_list
        //                         where _c.Customer_Id == 9
        //                         select _c).SingleOrDefault<contact_list_item>();



        //        return Json(new { result = "fail", details = "success" });
        //    }
        //    catch (Exception err)
        //    {
        //        return Json(new { result = "fail", details = err.Message });

        //    }
        //}
    }
}
