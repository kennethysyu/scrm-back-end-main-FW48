using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using scrm1.Models;
using System.Web.Http;
using System.Net.Http;
using scrm1.Class;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Permissions;

namespace scrm1.Controllers
{
    public class scrm_others: scrmOtherFunctions
    {
        // declare new entity object
        private scrmEntities _scrme = new scrmEntities();

        //private scrmListFunctions _scrm_list_functions = new TBD_scrm_lists();


        // JWT
        private static string Secret = "moKvdQJpSZOUmSuiitf4NfLTzoyaF0xdalc3tnDJQVokN9m+3eDVmWkdpwRL1Ogpb2+roauXzmpckJS/O4POrw==";

        public static string GenerateToken(string P_Username)
        {
            byte[] _non_base64_secret = Convert.FromBase64String(Secret);
            SymmetricSecurityKey _symmetric_security_key = new SymmetricSecurityKey(_non_base64_secret);

            ClaimsIdentity _claims_identity = new ClaimsIdentity();
            _claims_identity.AddClaim(new Claim(ClaimTypes.Name, P_Username));

            SecurityTokenDescriptor _security_token_descriptor = new SecurityTokenDescriptor
            {
                Subject = _claims_identity,
                Expires = DateTime.UtcNow.AddMinutes(60 * 24),
                SigningCredentials = new SigningCredentials(_symmetric_security_key, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler _jwt_security_token_handler = new JwtSecurityTokenHandler();
            JwtSecurityToken _jwt_security_token = _jwt_security_token_handler.CreateJwtSecurityToken(_security_token_descriptor);
            return _jwt_security_token_handler.WriteToken(_jwt_security_token);
        }

        public static string ValidateToken(string P_Token)
        {
            ClaimsIdentity _claims_identity;

            ClaimsPrincipal _claims_principal = GetClaimsPrincipal(P_Token);
            if (_claims_principal == null) return null;

            try
            {
                _claims_identity = (ClaimsIdentity)_claims_principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            };

            Claim _claim_name = _claims_identity.FindFirst(ClaimTypes.Name);
            return _claim_name.Value; // username
        }

        public static ClaimsPrincipal GetClaimsPrincipal(string P_Token)
        {
            try
            {
                JwtSecurityTokenHandler _jwt_security_token_handler = new JwtSecurityTokenHandler();
                JwtSecurityToken _jwt_security_token = (JwtSecurityToken)_jwt_security_token_handler.ReadToken(P_Token);

                if (_jwt_security_token == null) return null;

                byte[] _non_base64_secret = Convert.FromBase64String(Secret);

                TokenValidationParameters _token_validation_parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(_non_base64_secret)
                };

                SecurityToken _security_token;
                ClaimsPrincipal _claims_principal = _jwt_security_token_handler.ValidateToken(P_Token, _token_validation_parameters, out _security_token);

                return _claims_principal;
            }
            catch (Exception e)
            {
                return null;
            };
        }
        
        public bool authenticated(string token, string P_Username)
        {
            //string _username = ValidateToken(token);

            //if (P_Username == _username)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //};

            if (string.IsNullOrEmpty(token) ||
                string.IsNullOrEmpty(P_Username))
            {
                return false;
            }

            return ValidateToken(token) == P_Username;

        }



        //Login ldap
        public JObject loginldap([FromBody] dynamic data)
        {
            // obtain form body values
            //int agentId = (data.Agent_Id == null) ? -1 : (int)data.Agent_Id.Value;
            string sellerId = (data.SellerID == null) ? string.Empty : data.SellerID.Value;
            string password = data.Password.Value; // later change to base64

            //   bool canLogin = false;
            string details = string.Empty;

            // declare a json object to contain all rows of data
            JObject allJsonResults = new JObject();

            string ldapserver = System.Web.Configuration.WebConfigurationManager.AppSettings["Ldap_server"];
            int ldapport = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["Ldap_port"]);
            string ldappath = System.Web.Configuration.WebConfigurationManager.AppSettings["Ldap_path"];
            
            try
            {
                // Create the new LDAP connection
                LdapDirectoryIdentifier ldi = new LdapDirectoryIdentifier(ldapserver, ldapport);
                System.DirectoryServices.Protocols.LdapConnection ldapConnection =
                    new System.DirectoryServices.Protocols.LdapConnection(ldi);
                //    Console.WriteLine("LdapConnection is created successfully.");
                ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.SessionOptions.ProtocolVersion = 3;
                NetworkCredential nc = new NetworkCredential("uid=" + sellerId + ldappath, password); //password
                ldapConnection.Bind(nc);
                //       Console.WriteLine("LdapConnection authentication success");
                ldapConnection.Dispose();

                //allJsonResults = new JObject()
                //    {
                //        new JProperty("result", "success"),
                //        new JProperty("details", "LdapConnection authentication success"),

                //    };

                //return allJsonResults;


                // obtain all data from table agentInfo
                IQueryable<agent_info_list_item> _agent_details = (from _a in _scrme.agentinfo

                                                                   select _a);

                // declare an agent and temp agent record
                agent_info_list_item _agent = null;
                //       agent_info_list_item _agent_temp = null;


                // use seller id to log in
                if (sellerId != string.Empty)
                {
                    // obtain the single record with the seller id and password
                    //                    _agent = _agent_details.Where(_a => _a.SellerID == sellerId && _a.Password == password).SingleOrDefault<agent_info_list_item>();
                    _agent = _agent_details.Where(_a => _a.SellerID == sellerId && _a.Account_status == "Active").SingleOrDefault<agent_info_list_item>();

                    // obtain the single record with the seller id
                    //         _agent_temp = _agent_details.Where(_a => _a.SellerID == sellerId).SingleOrDefault<agent_info_list_item>();
                }


                if (_agent != null)
                {
                    _agent.LastLoginDate = DateTime.Now; // update LastLoginDate
                    // update status and save changes in db
                    _scrme.Entry(_agent).State = System.Data.Entity.EntityState.Modified;
                    _scrme.SaveChanges();

                    // declare a temp json object to store each agent item
                    JObject agentObj = new JObject();
                    agentObj.RemoveAll(); // clear the object

                    // iterate each column of the _agent_item
                    foreach (PropertyInfo property in _agent.GetType().GetProperties())
                    {
                        // add all column names and values to temp, except "Password"
                        switch (property.Name)
                        {
                            case "Password":
                            case "Photo":
                            case "Photo_Type":
                                {
                                    break;
                                }
                            default:
                                // add the column name and value to temp
                                agentObj.Add(new JProperty(property.Name, property.GetValue(_agent)));
                                break;
                        }
                    }

                    int? roleId = _agent.LevelID; // obtain the role id from agent_info

                    // use the role id to find the corresponding role name and companies
                    var _role = (from _r in _scrme.user_role
                                 where _r.RoleID == roleId
                                 select new
                                 {
                                     _r.RoleName,
                                     _r.Companies,
                                     _r.Categories,
                                     _r.Functions
                                 }).SingleOrDefault();


                    // add role name and companies to _agentObj
                    agentObj.Add(new JProperty("RoleName", _role.RoleName));
                    agentObj.Add(new JProperty("Companies", _role.Companies));
                    agentObj.Add(new JProperty("Categories", _role.Categories));
                    agentObj.Add(new JProperty("Functions", _role.Functions));

                    agentObj.Add(new JProperty("Token", GenerateToken(Convert.ToString(_agent.AgentID))));


                    // obtain all data from table config
                    IQueryable<config_list_item> _config_details = (from _conf in _scrme.config
                                                                    select _conf);

                    // declare new FieldDetails class object as List
                    List<ConfigDetails> _list_config_details = new List<ConfigDetails>();

                    // for existing config
                    if (_config_details.Count() > 0)
                    {
                        // iterate through the rows from that field category
                        foreach (config_list_item _config_item in _config_details)
                        {
                            // declare ConfigDetails class object
                            ConfigDetails _cd = new ConfigDetails();

                            // assign the id, name and details to the ConfigDetails object
                            _cd.P_Id = _config_item.P_Id;
                            _cd.P_Name = _config_item.P_Name;
                            _cd.P_Value = _config_item.P_Value;

                            // apend the object to the list
                            _list_config_details.Add(_cd);
                        }

                        // for non-existing field category
                    }
                    else
                    {
                        _list_config_details = null;
                    }

                    JArray configJson = (JArray)JToken.FromObject(_list_config_details);

                    // add config to _agentObj
                    agentObj.Add(new JProperty("config", configJson));
                    //_agentObj.Add(configJson);

                    allJsonResults = new JObject()
                    {
                        new JProperty("result", "success"),
                        new JProperty("details", agentObj)
                    };

                    // return all results in json format
                    return allJsonResults;

                }
                else
                {
                    allJsonResults = new JObject()
                    {
                        new JProperty("result", "fail"),
                        new JProperty("details", "Invalid login or account is inactive.")
                    };

                    // return all results in json format
                    return allJsonResults;
                }


            }
            catch (LdapException e)
            {
                //       Console.WriteLine("\r\nUnable to login:\r\n\t" + e.Message);
                allJsonResults = new JObject()
                    {
                        new JProperty("result", "fail"),
                       // new JProperty("details", "[Ldap] Unable to login:"  + e.Message),
                        new JProperty("details", "[Ldap] Unable to login"),
                    };

                return allJsonResults;
            }
            catch (Exception e)
            {
                //      Console.WriteLine("\r\nUnexpected exception occured:\r\n\t" + e.GetType() + ":" + e.Message);
                allJsonResults = new JObject()
                    {
                        new JProperty("result", "fail"),
                        new JProperty("details", " Unexpected exception occured:"  + e.Message),

                    };

                return allJsonResults;
            }


        }


        //Login
        public JObject login([FromBody] dynamic data)
        {
            // obtain form body values
            //int agentId = (data.Agent_Id == null) ? -1 : (int)data.Agent_Id.Value;
            string sellerId = (data.SellerID == null) ? string.Empty : data.SellerID.Value;
            string password = data.Password.Value; // later change to base64

            bool canLogin = false;
            string details = string.Empty;

            // obtain all data from table agentInfo
            IQueryable<agent_info_list_item> _agent_details = (from _a in _scrme.agentinfo 

                                                                select _a);

            // declare an agent and temp agent record
            agent_info_list_item _agent = null;
            agent_info_list_item _agent_temp = null;

            //// use agent id to log in
            //if (agentId != -1)
            //{
            //    // obtain the single record with the agent id and password
            //    _agent = _agent_details.Where(_a => _a.AgentID == agentId && _a.Password == password).SingleOrDefault<agent_info_list_item>();

            //    // obtain the single record with the agent id
            //    _agent_temp = _agent_details.Where(_a => _a.AgentID == agentId).SingleOrDefault<agent_info_list_item>();
            //}

            // use seller id to log in
            if (sellerId != string.Empty)
            {
                // obtain the single record with the seller id and password
                _agent = _agent_details.Where(_a => _a.SellerID == sellerId && _a.Password == password).SingleOrDefault<agent_info_list_item>();

                // obtain the single record with the seller id
                _agent_temp = _agent_details.Where(_a => _a.SellerID == sellerId).SingleOrDefault<agent_info_list_item>();
            }

            // obtain the single record with the agent id and password
            //agent_info_list_item _agent = _agent_details.Where(_a => _a.AgentID == agentId && _a.Password == password).SingleOrDefault<agent_info_list_item>();

            // declare a json object to contain all rows of data
            JObject allJsonResults = new JObject();

            //both id and password are correct
            if (_agent != null)
            {
                // the account status is also active and the login attempt is <= 3
                if (_agent.Account_status == "Active" && _agent.Counter < 3 && _agent.ExpiryDate > DateTime.Today && _agent.LastLoginDate != null)
                {
                    canLogin = true; //can login
                    _agent.Counter = 0; // reset counter
                    _agent.LastLoginDate = DateTime.Now; // update LastLoginDate

                    // update status and save changes in db
                    _scrme.Entry(_agent).State = System.Data.Entity.EntityState.Modified;
                    _scrme.SaveChanges();
                } else
                {
                    // set the alert messages and login status to false
                    if (_agent.Account_status != "Active")
                    {
                        details = "Account is inactive.";
                    }
                    else if (_agent.LastLoginDate == null)
                    {
                        details = "Initial Login.";
                    }
                    else if (_agent.ExpiryDate <= DateTime.Today)
                    {
                        details = "Account has expired.";
                    }
                    else if (_agent.Counter == 3)
                    {
                        details = "Account is locked.";
                    }

              //      _agent.Counter = 0; // reset counter

                    canLogin = false;
                }
            } else
            // either the id or password is incorrect
            {
                //// obtain the single record with the agent id
                //agent_info_list_item _agent_temp = _agent_details.Where(_a => _a.AgentID == agentId).SingleOrDefault<agent_info_list_item>();

                canLogin = false; // cannot login

                // wrong password
                if (_agent_temp != null)
                {
                    // only add counter when it's < 3
                    if (_agent_temp.Counter < 3)
                    {
                        // iterate the counter
                        _agent_temp.Counter = _agent_temp.Counter + 1;
                    } else
                    {
                        details = "Account is locked.";
                    }

                    // update status and save changes in db
                    _scrme.Entry(_agent_temp).State = System.Data.Entity.EntityState.Modified;
                    _scrme.SaveChanges();
                }
            }

            if (!canLogin)
            {
                if (details == "Initial Login.")
                {
                    allJsonResults = new JObject()
                    {
                        new JProperty("result", "fail"),
                        new JProperty("details", details),
                        new JProperty("AgentID", _agent.AgentID),
                        new JProperty("Token", GenerateToken(Convert.ToString(_agent.AgentID)))
                    };
                }
                else if (details == "Account has expired.")
                {
                    allJsonResults = new JObject()
                    {
                        new JProperty("result", "fail"),
                        new JProperty("details", details),
                        new JProperty("AgentID", _agent.AgentID),
                        new JProperty("Token", GenerateToken(Convert.ToString(_agent.AgentID)))
                    };
                }
                else
                {

                    details = (details == "") ? "Invalid login." : details;

                    allJsonResults = new JObject()
                    {
                        new JProperty("result", "fail"),
                        new JProperty("details", details)
                    };
                }
            }
            else
            {
                // declare a temp json object to store each agent item
                JObject agentObj = new JObject();
                agentObj.RemoveAll(); // clear the object

                // iterate each column of the _agent_item
                foreach (PropertyInfo property in _agent.GetType().GetProperties())
                {
                    // add all column names and values to temp, except "Password"
                    switch (property.Name)
                    {
                        case "Password":
                        case "Photo":
                        case "Photo_Type":
                            {
                                break;
                            }
                        default:
                            // add the column name and value to temp
                            agentObj.Add(new JProperty(property.Name, property.GetValue(_agent)));
                            break;
                    }
                }

                int? roleId = _agent.LevelID; // obtain the role id from agent_info

                // use the role id to find the corresponding role name and companies
                var _role = (from _r in _scrme.user_role
                            where _r.RoleID == roleId
                            select new
                            {
                                _r.RoleName,
                                _r.Companies,
                                _r.Categories,
                                _r.Functions
                            }).SingleOrDefault();


                // add role name and companies to _agentObj
                agentObj.Add(new JProperty("RoleName", _role.RoleName));
                agentObj.Add(new JProperty("Companies", _role.Companies));
                agentObj.Add(new JProperty("Categories", _role.Categories));
                agentObj.Add(new JProperty("Functions", _role.Functions));

                agentObj.Add(new JProperty("Token", GenerateToken(Convert.ToString(_agent.AgentID))));
                

                // obtain all data from table config
                IQueryable<config_list_item> _config_details = (from _conf in _scrme.config
                                       select _conf);

                // declare new FieldDetails class object as List
                List<ConfigDetails> _list_config_details = new List<ConfigDetails>();

                // for existing config
                if (_config_details.Count() > 0)
                {
                    // iterate through the rows from that field category
                    foreach (config_list_item _config_item in _config_details)
                    {
                        // declare ConfigDetails class object
                        ConfigDetails _cd = new ConfigDetails();

                        // assign the id, name and details to the ConfigDetails object
                        _cd.P_Id = _config_item.P_Id;
                        _cd.P_Name = _config_item.P_Name;
                        _cd.P_Value = _config_item.P_Value;
                        
                        // apend the object to the list
                        _list_config_details.Add(_cd);
                    }

                    // for non-existing field category
                }
                else
                {
                    _list_config_details = null;
                }

                JArray configJson = (JArray)JToken.FromObject(_list_config_details);

                // add config to _agentObj
                agentObj.Add(new JProperty("config", configJson));
                //_agentObj.Add(configJson);

                allJsonResults = new JObject()
                {
                    new JProperty("result", "success"),
                    new JProperty("details", agentObj)
                };
            }
            // return all results in json format
            return allJsonResults;
        }

        //Get all agentInfo data
        public JObject getLogin()
        {            
            // obtain linq results by left joining 2 tables: agentinto and user_role
            var _agent_users = from _agent in _scrme.agentinfo
                               join _role in _scrme.user_role
                                    on _agent.LevelID equals _role.RoleID
                                    into joined
                               from _roles in joined.DefaultIfEmpty()
                                   orderby _agent.SellerID
                               //where _agent.Account_status == "Active"
                               select new
                               {
                                   _agent,
                                   _roles.RoleName,
                                   //_roles.Companies
                               };

            // declare a list of json objects containing the each row of data
            List<JObject> _agent_list = new List<JObject>();

            // declare a json object to contain all rows of data
            JObject allJsonResults = new JObject();

            // iterate through each row of data in agentInfo
            foreach (var _agent_item in _agent_users)
            {
                // declare a temp json object to store each column of data
                JObject tempJson = new JObject();

                tempJson.RemoveAll(); // clear the temp object

                // iterate through each column of the _agent_item
                foreach (PropertyInfo property in _agent_item._agent.GetType().GetProperties())
                {
                    // add all column names and values to temp, except "Password"
                    switch (property.Name)
                    {
                        //case "Photo":
                        //case "AgentID":
                        //case "AgentName":
                        //case "Email":
                        ////case "LevelID":
                        //    {   // add the column name and value to temp
                        //        tempJson.Add(new JProperty(property.Name, property.GetValue(_agent_item._agent)));
                        //        break;
                        //    }

                        //default:
                        //    break;

                        //case "Password":
                        //    // encrypt the password
                        //    var encryptedPassword = new SecureString();
                        //    foreach (char c in _agent_item._agent.Password)
                        //    {
                        //        encryptedPassword.AppendChar(c);
                        //    }
                        //    tempJson.Add(new JProperty(property.Name, encryptedPassword));
                        //    break;

                        //case "ColId":
                        case "Password":
                        //case "Photo":
                        //case "Photo_Type":
                            {
                                break;
                            }

                        default:
                            //add the column name and value to temp
                            tempJson.Add(new JProperty(property.Name, property.GetValue(_agent_item._agent)));
                            break;
                    }
                }

                tempJson.Add(new JProperty("RoleName", _agent_item.RoleName));
                //tempJson.Add(new JProperty("Companies", _agent_item.Companies));

                _agent_list.Add(tempJson);
            }



            // set up _all_results json data
            allJsonResults = new JObject()
            {
                new JProperty("result", "success"),
                new JProperty("details", _agent_list)
            };

            // return all results in json format
            return allJsonResults;
        }

        public List<agent_info_list_item> createUser([FromBody] dynamic data)
        {
            // declare db table items
            agent_info_list_item _agent_item = new agent_info_list_item();

            // obtain form body values
            int agentId = (int)data.AgentID.Value;
            string sellerID = (data.SellerID == null) ? string.Empty : data.SellerID.Value;
            string agentName = (data.AgentName == null) ? string.Empty : data.AgentName.Value;
            string email = (data.Email == null) ? string.Empty : data.Email.Value;
            string password = (data.Password == null) ? string.Empty : data.Password.Value;
            string role = (data.Role == null) ? string.Empty : data.Role.Value;            
            int levelId = getLevelId(role); // get level Id using the role name
            string accountStatus = (data.Account_status == null) ? string.Empty : data.Account_status.Value;
            string photoRemoved = (data.Photo_Removed == null) ? "N" : data.Photo_Removed.Value;

            // assign new agent record
            _agent_item.AgentID = agentId;
            _agent_item.SellerID = sellerID;
            _agent_item.Counter = 0;
            _agent_item.AgentName = agentName;
            _agent_item.Email = email;
            _agent_item.Password = password;
            _agent_item.LevelID = levelId;
            _agent_item.Account_status = accountStatus;
            //_agent_item.ExpiryDate = DateTime.Today.AddDays(90);
            int passwordChangeFrequency = getPasswordChangeFrequency();
            _agent_item.ExpiryDate = DateTime.Today.AddDays(passwordChangeFrequency);

            if (photoRemoved == "Y")
            {
                _agent_item.Photo = null;
                _agent_item.Photo_Type = null;
            }
            
            // add new user role record
            _scrme.agentinfo.Add(_agent_item);

            // save db changes
            _scrme.SaveChanges();
            

            // obtain the new agent from table "agentinfo"
            List<agent_info_list_item> _new_agent = (from _a in _scrme.agentinfo
                                                          where _a.ColId == _agent_item.ColId
                                                          select _a).ToList();


            return _new_agent;
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

        public string getAgentsOfRole([FromBody] dynamic data)
        {
            int levelId = (data.LevelID == null) ? -1 : Convert.ToInt32(data.LevelID.Value);

            // obtain user records based on the level id
            IQueryable<agent_info_list_item> _agents = (from _a in _scrme.agentinfo
                                                          where _a.LevelID == levelId
                                                          select _a);

            string agentsOfRole = string.Empty;

            if (_agents.Count() > 0)
            {
                agentsOfRole = "The role is used by: ";
                // iterate through each row of data in agentInfo
                foreach (agent_info_list_item _agent_item in _agents)
                {
                    // append agent to the string
                    agentsOfRole = agentsOfRole + _agent_item.AgentName + "(ID: " + _agent_item.AgentID + ")\n";
                }
            }

            return agentsOfRole;
        }

        public bool sellerIdExists([FromBody] dynamic data)
        {
            string sellerId = (data.SellerID == null) ? string.Empty : data.SellerID.Value;

            bool exists = true;
            // obtain single user record based on the agent id
            agent_info_list_item _agent = (from _a in _scrme.agentinfo
                                              where _a.SellerID == sellerId
                                              select _a).SingleOrDefault<agent_info_list_item>();

            // if there is at least 1 seller
            if (_agent == null)
            {
                exists = false;
            }

            return exists;
        }

        public int getLevelId (string roleName)
        {
            int levelId = 0;

            user_role_list_item _linq_user_role = (from _r in _scrme.user_role
                                                  where _r.RoleName == roleName
                                                  select _r).SingleOrDefault();

            if(_linq_user_role != null)
            {
                levelId = _linq_user_role.RoleID;
            }

            return levelId;
        }

        //public void saveAgentPhoto(int agentId, byte[] photo, string fileType)
        //{
        //    // obtain the row of data with the given agent id
        //    agent_info_list_item _agent = (from _a in _scrme.agentinfo
        //                                   where _a.AgentID == agentId
        //                                   select _a).SingleOrDefault();

        //    _agent.Photo = photo; // assign the file to Photo column in table
        //    _agent.Photo_Type = fileType; // assign the file type to Photo_Type column
        //    _scrme.SaveChanges(); // save to database
        //}

        public string saveAgentPhoto(int agentId, byte[] photo, string fileType)
        {
            // obtain the row of data with the given agent id
            agent_info_list_item _agent = (from _a in _scrme.agentinfo
                                           where _a.AgentID == agentId
                                           select _a).SingleOrDefault();
            if (_agent == null)
            {
                return "fail";
            }
            else // contact exists
            {
                _agent.Photo = photo; // assign the file to Photo column in table
                _agent.Photo_Type = fileType; // assign the file type to Photo_Type column
                _scrme.SaveChanges(); // save to database

                return "success";
            }
        }

        public JObject getAgentPhoto([FromBody] dynamic data)
        {
            JObject photoJson = new JObject(); // declare json object

            // obtain agent id from form
            int agentId = (data.Agent_Id == null) ? -1 : Convert.ToInt32(data.Agent_Id.Value);

            if (agentId == -1)
            {
                photoJson = new JObject() // no customer
                    {
                        new JProperty("result", "fail"),
                        new JProperty("details", "invalid parameters")
                    };
            }
            else
            {
                // obtain the row of data with the given agent id
                agent_info_list_item _agent = (from _a in _scrme.agentinfo
                                               where _a.AgentID == agentId
                                               select _a).SingleOrDefault();

                if (_agent != null)
                {
                    if (_agent.Photo == null)
                    {
                        Array.Clear(_agent.Photo, 0, _agent.Photo.Length);
                        _agent.Photo_Type = string.Empty;
                    }
                    JObject jsonResults = new JObject()
                    {   // add column data to json object
                        new JProperty("Photo_Type", _agent.Photo_Type),
                        new JProperty("Photo_Content", Convert.ToBase64String(_agent.Photo))
                    };

                    photoJson = new JObject() // add to overall json object
                    {
                        new JProperty("result", "success"),
                        new JProperty("details", jsonResults)
                    };
                }
                else
                {
                    photoJson = new JObject() // add to overall json object
                    {
                        new JProperty("result", "fail"),
                        new JProperty("details", "agent does not exist")
                    };
                }
            }
            return photoJson;
        }

        public void updateUser([FromBody] dynamic data)
        {
            // obtain form body values
            int colId = (int)data.ColId.Value;
            int agentId = (int)data.AgentID.Value;
            string sellerID = (data.SellerID == null) ? string.Empty : data.SellerID.Value;
            string agentName = (data.AgentName == null) ? string.Empty : data.AgentName.Value;
            string email = (data.Email == null) ? string.Empty : data.Email.Value;
            string password = (data.Password == null) ? string.Empty : data.Password.Value;
            string role = (data.Role == null) ? string.Empty : data.Role.Value;
            int levelId = getLevelId(role); // get level Id using the role name
            string accountStatus = (data.Account_status == null) ? string.Empty : data.Account_status.Value;
            int counter = (data.Counter == null) ? -1 : (int)data.Counter.Value;
            string photoRemoved = (data.Photo_Removed == null) ? "N" : data.Photo_Removed.Value;

            // obtain single user record based on the agent id
            agent_info_list_item _agent = (from _a in _scrme.agentinfo
                                          where _a.ColId == colId
                                         select _a).SingleOrDefault<agent_info_list_item>();

            // if there is at least 1 role
            if (_agent != null)
            {
                // decide if it's update with agent id or update w/o agent id
                if (_agent.AgentID == agentId && _agent.SellerID == sellerID)
                {
                    // assign the updated values to the row
                    //_agent.AgentID = agentId;
                    //_agent.SellerID = sellerID;
                    _agent.AgentName = agentName;
                    _agent.Email = email;
                    if (password != string.Empty)
                    {
                        _agent.Password = password;
                        int passwordChangeFrequency = getPasswordChangeFrequency();
                        _agent.ExpiryDate = DateTime.Today.AddDays(passwordChangeFrequency); // only extend expiry date if password is changed
                    }
                    _agent.LevelID = levelId;
                    _agent.Account_status = accountStatus;
                    _agent.Counter = counter;

                    if (photoRemoved == "Y")
                    {
                        _agent.Photo = null;
                        _agent.Photo_Type = null;
                    }
                    // plain update because agent id has not changed          
                    _scrme.Entry(_agent).State = System.Data.Entity.EntityState.Modified; // update status and save changes in db
                } else
                {
                    // add a new row using the data above
                    // declare db table items
                    agent_info_list_item _new_agent_item = new agent_info_list_item();

                    // assign new agent record
                    _new_agent_item.AgentID = agentId;
                    _new_agent_item.AgentName = agentName;
                    _new_agent_item.Password = _agent.Password;
                    _new_agent_item.LevelID = _agent.LevelID;
                    _new_agent_item.SellerID = sellerID;
                    _new_agent_item.Counter = _agent.Counter;
                    int passwordChangeFrequency = getPasswordChangeFrequency();
                    // _new_agent_item.ExpiryDate = _agent.ExpiryDate;
                    _new_agent_item.ExpiryDate = DateTime.Today.AddDays(passwordChangeFrequency);
                    _new_agent_item.LastLoginDate = _agent.LastLoginDate;
                    _new_agent_item.Account_status = _agent.Account_status;
                    _new_agent_item.Email = _agent.Email;

                    if (photoRemoved == "Y")
                    {
                        _new_agent_item.Photo = null;
                        _new_agent_item.Photo_Type = null;
                    }

                    // delete the old row
                    _scrme.agentinfo.Remove(_agent);

                    // add the new row
                    _scrme.agentinfo.Add(_new_agent_item);
                }
                _scrme.SaveChanges(); // save changes to db
            }
        }

        public int getPasswordChangeFrequency()
        {
            config_list_item _config = (from _c in _scrme.config
                                        where _c.P_Name == "PasswordChangeFrequency"
                                        select _c).SingleOrDefault<config_list_item>();

            if (_config == null)
            {
                return 90;
            } else
            {
                return Convert.ToInt32(_config.P_Value);
            }
        }

        public int getPasswordReUseTimes()
        {
            config_list_item _config = (from _c in _scrme.config
                                        where _c.P_Name == "PasswordReUseTimes"
                                        select _c).SingleOrDefault<config_list_item>();

            if (_config == null)
            {
                return 5;
            }
            else
            {
                return Convert.ToInt32(_config.P_Value);
            }
        }


        public string changePassword([FromBody] dynamic data)
        {
            // obtain form body values
            //int agentId = (int)data.AgentID.Value;
            string sellerId = (data.SellerID == null) ? string.Empty : data.SellerID.Value;
            string oldPassword = (data.Old_Password == null) ? string.Empty : data.Old_Password.Value;
            string password = (data.Password == null) ? string.Empty : data.Password.Value;

            // obtain single user record based on the agent id
            agent_info_list_item _agent = (from _a in _scrme.agentinfo
                                              where _a.SellerID == sellerId
                                              select _a).SingleOrDefault<agent_info_list_item>();

            // agent exists
            if (_agent != null)
            {
                if (_agent.Password == oldPassword)
                {
                    int pwd_reuse_times = getPasswordReUseTimes();

                    IQueryable<String> _h;

                    _h = (from _a in _scrme.password_log
                                 where _a.AgentID == _agent.AgentID
                                 orderby _a.Created_Time descending
                                 select _a.Password ).Take(pwd_reuse_times);

                    List<String> _h_list = _h.ToList<String>();

                    bool isDup;

                    isDup = _h_list.Contains(password);

                    if (isDup)
                    {
                        return "The new password has been used before.";
                    }
                    else
                    {
                        // assign the updated values to the row
                        _agent.Password = password;
                        int passwordChangeFrequency = getPasswordChangeFrequency();
                        _agent.ExpiryDate = DateTime.Now.AddDays(passwordChangeFrequency); // extend the account expiry date

                        _agent.LastLoginDate = DateTime.Now;

                        // plain update because agent id has not changed          
                        _scrme.Entry(_agent).State = System.Data.Entity.EntityState.Modified; // update status and save changes in db

                        _scrme.SaveChanges(); // save changes to db

                        // Insert password log
                        password_log_list_item _new_pw_item = new password_log_list_item();

                        _new_pw_item.Password = password;
                        _new_pw_item.AgentID = _agent.AgentID;
                        _new_pw_item.Created_Time = DateTime.Now;

                        _scrme.password_log.Add(_new_pw_item);

                        _scrme.SaveChanges();


                        return "password is changed.";
                    }

                } else
                {
                    return "The existing password does not match with our record.";
                }
            } else
            {
                return "Invalid seller ID.";
            }
        }

        //public JObject getPhoto([FromBody] dynamic data)
        //{
        //    JObject photoJson = new JObject(); // declare json object

        //    // obtain customer id from form
        //    int customerId = (data.Customer_Id == null) ? -1 : Convert.ToInt32(data.Customer_Id.Value);

        //    if (customerId == 0)
        //    {
        //        photoJson = new JObject() // no customer
        //            {
        //                new JProperty("result", "fail"),
        //                new JProperty("details", "invalid parameters")
        //            };
        //    }
        //    else
        //    {
        //        // declare table item and obtain the row of data with the given customer id
        //        contact_list_item _contact = (from _c in _scrme.contact_list
        //                                      where _c.Customer_Id == customerId
        //                                      select _c).SingleOrDefault();

        //        if (_contact != null)
        //        {
        //            JObject jsonResults = new JObject()
        //            {   // add column data to json object
        //                new JProperty("Photo_Type", _contact.Photo_Type),
        //                new JProperty("Photo_Content", Convert.ToBase64String(_contact.Photo))
        //            };

        //            photoJson = new JObject() // add to overall json object
        //            {
        //                new JProperty("result", "success"),
        //                new JProperty("details", jsonResults)
        //            };
        //        } else
        //        {
        //            photoJson = new JObject() // add to overall json object
        //            {
        //                new JProperty("result", "fail"),
        //                new JProperty("details", "customer does not exist")
        //            };
        //        }     
        //    }
        //    return photoJson;
        //}

        // GetFields
        public Dictionary<string, List<FieldDetails>> getFields([FromBody] dynamic data)
        {
            // obtain form body values
            dynamic listArray = data.listArr;

            // obtain linq result lists of table Field
            List<field_list_item> _linq_field = (from _f in _scrme.field
                                                 select _f).ToList();

            // obtain linq result lists of table Field_Option
            List<field_option_list_item> _linq_field_option = (from _f in _scrme.field_option
                                                               select _f).ToList();

            // declare a dictionary object where key = Field_Category, value = FieldDetails's values
            Dictionary<string, List<FieldDetails>> tableFieldsDict = new Dictionary<string, List<FieldDetails>>();

            // iterate through the 3 field categories
            foreach (string _field_category in listArray)
            {

                // declare new FieldDetails class object as List
                List<FieldDetails> _list_field_details = new List<FieldDetails>();

                // retrieve rows from a particular field category
                IOrderedEnumerable<field_list_item> _field_record = _linq_field.Where(_f => _f.Field_Category == _field_category).OrderBy(_f => _f.Field_Display);

                // for existing field category
                if (_field_record.Count() > 0)
                {
                    // iterate through the rows from that field category
                    foreach (field_list_item _field_item in _field_record)
                    {
                        // retrieve rows from a particular field name
                        IEnumerable<field_option_list_item> _option_record = _linq_field_option.Where(_o => (_o.Field_Name == _field_item.Field_Name));

                        // declare FieldDetails class object
                        FieldDetails _fd = new FieldDetails();

                        // assign the name, display and tag to the FieldDetails object
                        _fd.Field_Name = _field_item.Field_Name;
                        _fd.Field_Display = _field_item.Field_Display;
                        _fd.Field_Tag = _field_item.Field_Tag;
                        _fd.Field_Type = _field_item.Field_Type;

                        // declare an empty list called temp
                        List<string> temp = new List<string>();

                        // replace the Field_Options list with temp
                        _fd.Field_Options = temp;

                        // obtain fields options if there is any
                        if (_field_item.Field_Tag == "select")
                        {
                            // clear the temp list
                            temp.Clear();

                            // iterate through each option in table Field_Option
                            foreach (field_option_list_item _option in _option_record)
                            {
                                // add the option value to temp
                                temp.Add(_option.Field_Option1);
                            }

                            // replace the Field_Options list with temp
                            _fd.Field_Options = temp;
                        }
                        else
                        {   // for tags other than "select", add "N/A" to the list
                            temp.Add("N/A");
                        }

                        // apend the object to the list
                        _list_field_details.Add(_fd);
                    }

                    // for non-existing field category
                }
                else
                {
                    _list_field_details = null;
                }

                // add the category and its details to the dictionary
                tableFieldsDict.Add(_field_category, _list_field_details);
            }

            return tableFieldsDict;
        }

        public List<user_role_list_item> createRole([FromBody] dynamic data)
        {
            // declare db table items
            user_role_list_item _user_role_item = new user_role_list_item();

            // obtain form body values
            string roleName = (data.RoleName == null) ? string.Empty : data.RoleName.Value;
            string companies = (data.Companies == null) ? string.Empty : data.Companies.Value;
            string categories = (data.Categories == null) ? string.Empty : data.Categories.Value;
            string functions = (data.Functions == null) ? string.Empty : data.Functions.Value;
            string roleStatus = (data.RoleStatus == null) ? string.Empty : data.RoleStatus.Value;

            // assign new user role record
            _user_role_item.RoleName = roleName;
            _user_role_item.Companies = companies;
            _user_role_item.Categories = categories;
            _user_role_item.Functions = functions;
            _user_role_item.RoleStatus = roleStatus;
            //if (roleStatus == "Active")
            //{
            //    _user_role_item.RoleStatus = "A"; // set to active
            //} else
            //{
            //    _user_role_item.RoleStatus = "D"; // set to active
            //}

            // add new user role record
            _scrme.user_role.Add(_user_role_item);

            // save db changes
            _scrme.SaveChanges();

            int roleID = _user_role_item.RoleID;

            // obtain the new user role from table "user_role"
            List<user_role_list_item> _linq_user_role = (from _r in _scrme.user_role
                                                         where _r.RoleID == roleID
                                                         select _r).ToList();

            return _linq_user_role;
        }

        public JObject getRoles(string status)
        {
            List<JObject> roleListJason = new List<JObject>(); // declare overall json list

            JObject rolesJson = new JObject(); // declare json object

            IQueryable<user_role_list_item> _linq_user_roles = from _r in _scrme.user_role
                                   //where _r.RoleStatus == "A"
                                   select _r; // declare user role data

            if (status == "Active")
            {
                _linq_user_roles = _linq_user_roles.Where(_r => _r.RoleStatus == "A");
            }

            foreach (user_role_list_item _role_item in _linq_user_roles)
            {
                // declare a temp json object to store each column of data
                JObject tempJson = new JObject();
                tempJson.RemoveAll(); // clear the temp object

                // iterate through each column of the _agent_item
                foreach (PropertyInfo property in _role_item.GetType().GetProperties())
                {
                    tempJson.Add(new JProperty(property.Name, property.GetValue(_role_item)));

                }
                roleListJason.Add(tempJson);
            }

            rolesJson = new JObject() // add to overall json object
                    {
                        new JProperty("result", "success"),
                        new JProperty("details", roleListJason)
                    };

            return rolesJson;
        }

        public void updateRole([FromBody] dynamic data)
        {
            // obtain form body values
            int roleID = (int)data.RoleID.Value;

            // declare a dictionary object where key = fieldName, value = fieldValue
            Dictionary<string, string> fieldsToBeUpdatedDict = new Dictionary<string, string>();

            // iterate through data and add the field names and field values to the dictionary
            foreach (dynamic item in data)
            {
                // obtain form parameters to local variables
                string fieldName = item.Name;
                string fieldValue = item.Value;

        //      if (fieldName != "RoleID" )
                if (fieldName != "RoleID" && fieldName != "Agent_Id" && fieldName != "Token")
                {
                    fieldsToBeUpdatedDict.Add(fieldName, fieldValue); // add non-RoleID fields to the Dictionary
                }                
            }

            // obtain single role record based on the role id
            user_role_list_item _role = (from _r in _scrme.user_role
                                         where _r.RoleID == roleID
                                         select _r).SingleOrDefault<user_role_list_item>();

            // if there is at least 1 role
            if (_role != null)
            {
                // iterate through the dictionary
                foreach (KeyValuePair<string, string> fields in fieldsToBeUpdatedDict)
                {
                    // find the column name that matches with the field name in dictionary
                    PropertyInfo properInfo = _role.GetType().GetProperty(fields.Key);

                    // set the field value
                    properInfo.SetValue(_role, (fields.Value == null ? string.Empty : fields.Value));
                }

                // update status and save changes in db
                _scrme.Entry(_role).State = System.Data.Entity.EntityState.Modified;
                _scrme.SaveChanges();
            }
            //string roleName = data.RoleName.Value;
            //string companies = data.Companies.Value;
            //string categories = data.Categories.Value;
            //string functions = data.Functions.Value;
            //string roleStatus = (data.RoleStatus == null) ? "D" : data.RoleStatus.Value;

            //// see if role already exists
            //var _role = _linq_user_roles.Where(_r => _r.RoleID == roleID).FirstOrDefault();

            //// assign the updated values to the row
            //_role.RoleName = roleName;
            //_role.Companies = companies;
            //_role.Categories = categories;
            //_role.Functions = functions;
            //_role.RoleStatus = roleStatus;

            //// update status and save changes in db
            //_scrme.Entry(_role).State = System.Data.Entity.EntityState.Modified;
            //_scrme.SaveChanges();
        }


        public void addScheduleSetting([FromBody] dynamic data)
        {
            int agentId = (int)data.Agent_Id.Value;

            task_schedule_setting_list_item _new_sch_item = new task_schedule_setting_list_item();

            _new_sch_item.Service = data.Service.Value;
            _new_sch_item.Display_Message = data.Display_Message.Value;
            _new_sch_item.Schedule_Type = data.Schedule_Type.Value;
            _new_sch_item.Schedule_Time = Convert.ToDateTime(data.Schedule_Time.Value);
            _new_sch_item.Status = "Active";
            
            _new_sch_item.Created_By = agentId;
            _new_sch_item.Created_Time = DateTime.Now;
            _new_sch_item.Updated_By = agentId;
            _new_sch_item.Updated_Time = DateTime.Now;

            _scrme.task_schedule_setting.Add(_new_sch_item);

            _scrme.SaveChanges();

        }


        public void updateScheduleSetting([FromBody] dynamic data)
        {
            int sID = (int)data.S_Id.Value;
            int agentId = (int)data.Agent_Id.Value;
            string s_action = data.S_Action == null ? string.Empty : data.S_Action.Value;

            var _ss = (from _c in _scrme.task_schedule_setting
                       where _c.S_Id == sID && _c.Status == "Active"
                       select _c).SingleOrDefault<task_schedule_setting_list_item>();

            // exists in table
            if (_ss != null)
            {
                if (s_action == "Amend")
                {
                    _ss.Service = data.Service.Value;
                    _ss.Display_Message = data.Display_Message.Value;
                    _ss.Schedule_Type = data.Schedule_Type.Value;
                    _ss.Schedule_Time = Convert.ToDateTime(data.Schedule_Time.Value);

                    _ss.Updated_By = agentId;
                    _ss.Updated_Time = DateTime.Now;

                    // update and save changes in db
                    _scrme.Entry(_ss).State = System.Data.Entity.EntityState.Modified;
                    _scrme.SaveChanges();
                }
                else if (s_action == "Delete")
                {
                    _ss.Status = "InActive";

                    _ss.Updated_By = agentId;
                    _ss.Updated_Time = DateTime.Now;

                    // update and save changes in db
                    _scrme.Entry(_ss).State = System.Data.Entity.EntityState.Modified;
                    _scrme.SaveChanges();
                }

            }

        }


        public List<task_schedule_setting_list_item> getScheduleSetting()
        {

            // obtain results
            IQueryable<task_schedule_setting_list_item> _sch = from _r in _scrme.task_schedule_setting
                                                               where _r.Status == "Active"
                                                               select _r;

            return _sch.ToList();

        }


        public List<task_schedule_record_list_item> checkScheduleAlert()
        {

            // obtain results
            IQueryable<task_schedule_record_list_item> _sch = from _r in _scrme.task_schedule_record
                                                              where _r.Handle_By == null
                                                              select _r;

            return _sch.ToList();

        }

        
        public void handleScheduleAlert([FromBody] dynamic data)
        {
            int rID = (int)data.R_Id.Value;
            int agentId = (int)data.Agent_Id.Value;

            var _sr = (from _c in _scrme.task_schedule_record
                       where _c.R_Id == rID && _c.Handle_By == null
                       select _c).SingleOrDefault<task_schedule_record_list_item>();

            // exists in table
            if (_sr != null)
            {
                _sr.Comment = data.Comment.Value;

                _sr.Handle_By = agentId;
                _sr.Handle_Time = DateTime.Now;

                // update and save changes in db
                _scrme.Entry(_sr).State = System.Data.Entity.EntityState.Modified;
                _scrme.SaveChanges();

            }

        }


        public List<task_schedule_record_list_item> getScheduleHistory(string stype)
        {

            // obtain results
            IQueryable<task_schedule_record_list_item> _sch = from _r in _scrme.task_schedule_record
                                                              where _r.Handle_By != null
                                                              select _r;
            
            if (stype == "1month")
            {
                DateTime s_time = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00:00")).AddMonths(-1);

                _sch = _sch.Where(_r => _r.Handle_Time >= s_time);
            }
            else if (stype == "1year")
            {
                DateTime s_time = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00:00")).AddMonths(-12);

                _sch = _sch.Where(_r => _r.Handle_Time >= s_time);
            }
            
            return _sch.ToList();

        }

        
        public JObject getFloorPlan(string ftype, int fid)
        {
            var _info = (dynamic)null;

            if (ftype == "full")
            {
                _info = (from _m in _scrme.floor_plan
                         where _m.F_Id == fid
                         select _m);
            }
            else
            {
                _info = (from _m in _scrme.floor_plan
                         select new
                         {
                             F_Id = _m.F_Id,
                             Name = _m.Name,
                             Ordering = _m.Ordering,
                             Status = _m.Status
                         }).Take(500);
            }


            // declare a json object to contain all rows of data
            JObject allJsonResults = new JObject();

            // declare a list of json objects containing the each row of data
            List<JObject> jsonList = new List<JObject>();


            // return results in list or null
            //if (_info.Count() > 0)
            if (_info != null)
            {
                // iterate through 
                foreach (var _item in _info)
                {
                    // declare a temp json object to store each 
                    JObject tempJson = new JObject();
                    tempJson.RemoveAll(); // clear the object

                    // iterate each column 
                    foreach (PropertyInfo property in _item.GetType().GetProperties())
                    {

                        tempJson.Add(new JProperty(property.Name, property.GetValue(_item)));
                    }

                    jsonList.Add(tempJson); // add the temp result to the list
                }

                // add the log list to jobject
                allJsonResults = new JObject()
                {
                     new JProperty("result", "success"),
                     new JProperty("details", jsonList)
                };

            }
            else
            {
                allJsonResults = new JObject()
                {
                     new JProperty("result", "success"),
                     new JProperty("details", null)
                };
            }

            return allJsonResults;
        }


        public void addFloorPlan([FromBody] dynamic data)
        {
            int agentId = (int)data.Agent_Id.Value;

            floor_plan_list_item _new_fp_item = new floor_plan_list_item();

            _new_fp_item.Name = data.Name.Value;
            _new_fp_item.Ordering = (int)data.Ordering.Value;
            _new_fp_item.Value = data.Value.Value;
            _new_fp_item.Background = data.Background.Value;
            _new_fp_item.Style = data.Style.Value;
            _new_fp_item.Remarks = data.Remarks.Value;
            _new_fp_item.Status = "Active";

            _new_fp_item.Created_By = agentId;
            _new_fp_item.Created_Time = DateTime.Now;
            _new_fp_item.Updated_By = agentId;
            _new_fp_item.Updated_Time = DateTime.Now;

            _scrme.floor_plan.Add(_new_fp_item);

            _scrme.SaveChanges();

        }


        public void updateFloorPlan([FromBody] dynamic data)
        {
            int fID = (int)data.F_Id.Value;
            int agentId = (int)data.Agent_Id.Value;

            var _ss = (from _c in _scrme.floor_plan
                       where _c.F_Id == fID
                       select _c).SingleOrDefault<floor_plan_list_item>();

            // exists in table
            if (_ss != null)
            {
                _ss.Name = data.Name.Value;
                _ss.Ordering = (int)data.Ordering.Value;
                _ss.Value = data.Value.Value;
                _ss.Background = data.Background.Value;
                _ss.Style = data.Style.Value;
                _ss.Remarks = data.Remarks.Value;
                _ss.Status = data.Status.Value;

                _ss.Updated_By = agentId;
                _ss.Updated_Time = DateTime.Now;

                // update and save changes in db
                _scrme.Entry(_ss).State = System.Data.Entity.EntityState.Modified;
                _scrme.SaveChanges();

            }

        }



    }
}