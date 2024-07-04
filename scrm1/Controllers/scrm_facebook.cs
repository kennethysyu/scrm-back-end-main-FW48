using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using scrm1.Models;
using System.Web.Http;
using scrm1.Class;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace scrm1.Controllers
{
    public class scrm_facebook : scrmFacebookFunctions
    {
        // declare new entity object
        private scrmEntities _scrme = new scrmEntities();

        public List<facebook_post_list_item> createFacebookPostContent([FromBody] dynamic data)
        {
            // declare db table items
            facebook_post_list_item _post_item = new facebook_post_list_item();

            // obtain form body values
            int ticketId = (int)data.Ticket_Id.Value;
            int agentId = (int)data.Agent_Id.Value;
            string details = (data.Details == null) ? string.Empty : data.Details.Value;
            //string mediaType = (data.Media_Type == null) ? string.Empty : data.Media_Type.Value;
            //string mediaLink = (data.Media_Link == null) ? string.Empty : data.Media_Link.Value;

            // assign new agent record
            _post_item.Ticket_Id = ticketId;
            _post_item.Created_By = agentId;
            _post_item.Created_Time = DateTime.Now;
            _post_item.Updated_By = agentId;
            _post_item.Updated_Time = DateTime.Now;
            _post_item.Details = details;
            //_post_item.Media_Type = mediaType;
            //_post_item.Media_Link = mediaLink;

            // add new user role record
            _scrme.facebook_post.Add(_post_item);

            // save db changes
            _scrme.SaveChanges();


            // obtain the new agent from table "facebook_post"
            List<facebook_post_list_item> _new_post = (from _f in _scrme.facebook_post
                                                     where _f.Fb_Id == _post_item.Fb_Id
                                                     select _f).ToList();


            return _new_post;
        }

        public bool userExists([FromBody] dynamic data)
        {
            int agentId = (data.AgentID == null) ? -1 : Convert.ToInt32(data.AgentID.Value);

            bool exists = true;
            // obtain single user record based on the agent id
            agent_info_list_item _agent = (from _a in _scrme.agentinfo
                                          where _a.AgentID == agentId
                                          select _a).SingleOrDefault<agent_info_list_item>();

            // if there is at least 1 agent
            if (_agent == null)
            {
                exists = false;
            }

            return exists;
        }

        public bool ticketIdExists([FromBody] dynamic data)
        {
            int ticketId = (data.Ticket_Id == null) ? -1 : Convert.ToInt32(data.Ticket_Id.Value);

            bool exists = true;

            // obtain the row of data with the given ticket id
            facebook_post_list_item _post = (from _f in _scrme.facebook_post
                                             where _f.Ticket_Id == ticketId
                                             select _f).SingleOrDefault();

            // if there is at least 1 post
            if (_post == null)
            {
                exists = false;
            }

            return exists;
        }

        public void saveFbMedia(int ticketId, int agentId, string mediaType, string mediaLink)
        {
            // obtain the row of data with the given ticket id
            facebook_post_list_item _post = (from _f in _scrme.facebook_post
                                             where _f.Ticket_Id == ticketId
                                             select _f).SingleOrDefault();

            // assign the parameters to db columns
            _post.Media_Type = mediaType;
            _post.Media_Link = mediaLink;
            _post.Updated_By = agentId;
            _post.Updated_Time = DateTime.Now;
            _scrme.SaveChanges(); // save to database
        }

        public JObject getFacebookPostContent(int ticketId)
        {
            // declare a json object to contain the overall results
            JObject jsonResults = new JObject();

            // declare a json object to contain the details of the data
            List<JObject> detailsJson = new List<JObject>();

            IQueryable<facebook_post_list_item> _posts = (from _p in _scrme.facebook_post select _p);

            if (ticketId != -1)
            {
                _posts = _posts.Where(_p => _p.Ticket_Id == ticketId);
            }

            if (_posts.Count() > 0)
            {
                // iterate through each row of data in agentInfo
                foreach (facebook_post_list_item _post_item in _posts)
                {
                    // declare a temp json object to store each column of data
                    JObject tempJson = new JObject();

                    tempJson.RemoveAll(); // clear the temp object

                    // iterate each column of _post_content
                    foreach (PropertyInfo property in _post_item.GetType().GetProperties())
                    {
                        switch (property.Name)
                        {
                            case "Fb_Id":
                            case "Ticket_Id":
                            case "Details":
                            case "Media_Type":
                            case "Media_Link":
                                {
                                    tempJson.Add(new JProperty(property.Name, property.GetValue(_post_item)));
                                }
                                break;
                            default:
                                // For others, skip
                                break;
                        }
                    }
                    detailsJson.Add(tempJson); // add the temp result to the list
                }

                
            }

            //// get post content record using the ticket id
            //var _post_content = (from _post in _scrme.facebook_post
            //                    where _post.Ticket_Id == ticketId
            //                    select _post).SingleOrDefault();

            //if (_post_content != null)
            //{
            //    JObject tempJson = new JObject();
            //    // iterate each column of _post_content
            //    foreach (PropertyInfo property in _post_content.GetType().GetProperties())
            //    {
            //        switch (property.Name)
            //        {
            //            case "Ticket_Id":
            //            case "Details":
            //            case "Media_Type":
            //            case "Media_Link":
            //                {
            //                    tempJson.Add(new JProperty(property.Name, property.GetValue(_post_content)));
            //                }
            //                break;
            //            default:
            //                // For others, skip
            //                break;
            //        }
            //    }
            //    detailsJson.Add(tempJson); // add the temp result to the list
            //}

            // include the list in the JSON object of the final result
            jsonResults = new JObject()
            {
                new JProperty("result", "success"),
                new JProperty("details", detailsJson)
            };

            // return all results in json format
            return jsonResults;

        }

        public void updateFacebookPostContent([FromBody] dynamic data)
        {
            // obtain form body values
            int fbId = (int)data.Fb_Id.Value;
            int ticketId = (int)data.Ticket_Id.Value;
            int agentId = (int)data.Agent_Id.Value;
            string details = (data.Details == null) ? string.Empty : data.Details.Value;
            string mediaRemoved = (data.Media_Removed == null) ? "N" : data.Media_Removed.Value;
            
            //string mediaType = (data.Media_Type == null) ? string.Empty : data.Media_Type.Value;
            //string mediaLink = (data.Media_Link == null) ? string.Empty : data.Media_Link.Value;

            // obtain single post record based on the fb id
            facebook_post_list_item _post_item = (from _f in _scrme.facebook_post
                                                 where _f.Fb_Id == fbId
                                                 select _f).SingleOrDefault<facebook_post_list_item>();

            // if there is at least 1 post
            if (_post_item != null)
            {
                // decide if it's update with ticket id or update w/o ticket id
                if (_post_item.Ticket_Id == ticketId)
                {
                    // assign the updated values to the row
                    _post_item.Updated_By = agentId;
                    _post_item.Updated_Time = DateTime.Now;
                    _post_item.Details = details;

                    if (mediaRemoved == "Y")
                    {
                        _post_item.Media_Type = string.Empty;
                        _post_item.Media_Link = string.Empty;
                    }
                    //_post_item.Media_Type = mediaType;
                    //_post_item.Media_Link = mediaLink;

                    // plain update because agent id has not changed          
                    _scrme.Entry(_post_item).State = System.Data.Entity.EntityState.Modified; // update status and save changes in db
                }
                else
                {
                    // add a new row using the data above
                    // declare db table items
                    facebook_post_list_item _new_post_item = new facebook_post_list_item();

                    // assign new post record
                    _new_post_item.Ticket_Id = ticketId;
                    _new_post_item.Created_By = _post_item.Created_By;
                    _new_post_item.Created_Time = _post_item.Created_Time;
                    _new_post_item.Updated_By = agentId;
                    _new_post_item.Updated_Time = DateTime.Now;
                    _new_post_item.Details = details;
                    _new_post_item.Media_Link = _post_item.Media_Link;
                    _new_post_item.Media_Type = _post_item.Media_Type;

                    if (mediaRemoved == "Y")
                    {
                        _new_post_item.Media_Link = string.Empty;
                        _new_post_item.Media_Type = string.Empty;
                    }

                    // delete the old row
                    _scrme.facebook_post.Remove(_post_item);

                    // add the new row
                    _scrme.facebook_post.Add(_new_post_item);
                }
                _scrme.SaveChanges(); // save changes to db
            }
        }
    }
}