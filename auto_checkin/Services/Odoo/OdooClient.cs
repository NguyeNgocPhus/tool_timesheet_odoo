using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace auto_checkin.Services.Odoo
{
    public class OdooClient : OdooBaseClient
    {


        string access_token = "";
        string session_id = "";

        public OdooClient(string access_token = "", string session_id = "")
        {
            Access_token = access_token;
            Session_id = session_id;
        }

        public string Access_token { get => access_token; set => access_token = value; }
        public string Session_id { get => session_id; set => session_id = value; }

        public JObject excuteRequest(string method, string endPoint, Dictionary<string, dynamic> param)
        {
            if (param == null)
            {
                param = new Dictionary<string, dynamic>();
            }

            Dictionary<string, string> headers = APIConfig.createDefaultHeader();
            //if (headers.ContainsKey("access_token"))
            //{
            //    headers.Remove("access_token");
            //}
            //headers.Add("access_token", this.Access_token);

            if (!string.IsNullOrEmpty(session_id))
            {
                param.Add("session_id", session_id);
            }
            string response;

            if ("GET".Equals(method.ToUpper()))
            {
                response = sendHttpGetRequest(endPoint, param, headers);
            }
            else
            {
                response = sendHttpPostRequestWithBody(endPoint, param, param["body"], headers);
            }

            JObject result = null;
            try
            {
                result = JObject.Parse(response);
            }
            catch (Exception e)
            {
                throw new APIException("Response is not json: " + response);
            }
            return result;
        }


        public JObject getUserDetail()
        {
            JObject result = new JObject();
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            JObject body = JObject.FromObject(new
            {
                jsonrpc = "2.0",
                method = "call",
                @params = new
                {
                    args = new List<string>(),
                    model = "hr.employee",
                    method = "name_search",
                    kwargs = new
                    {
                        name = "",
                        args = new string[] { },
                        @operator = "ilike",
                        limit = 100,
                        context = new
                        {
                            lang = "en_US",
                            tz = "Asia/Ho_Chi_Minh",
                            uid = 65
                        }
                    }

                }
            });
            param.Add("body", body.ToString());

            result = excuteRequest("POST", "https://erp.thgdx.vn/web/dataset/call_kw/hr.employee/name_search", param);

            return result;
        }
        public JObject getProjects()
        {
            JObject result = new JObject();
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            JObject body = JObject.FromObject(new
            {
                jsonrpc = "2.0",
                method = "call",
                @params = new
                {
                    args = new List<string>(),
                    model = "project.project",
                    method = "name_search",
                    kwargs = new
                    {
                        name = "",
                        args = new string[] {
                        },
                        @operator = "ilike",
                        limit = 100,
                        context = new
                        {
                            lang = "en_US",
                            tz = "Asia/Ho_Chi_Minh",
                            uid = 65
                        }
                    }

                }
            });
            param.Add("body", body.ToString());

            result = excuteRequest("POST", "https://erp.thgdx.vn/web/dataset/call_kw/project.project/name_search", param);

            return result;
        }
        public JObject getTasks(int projectId)
        {
            JObject result = new JObject();
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            JObject body = JObject.FromObject(new
            {
                jsonrpc = "2.0",
                method = "call",
                @params = new
                {
                    args = new object[][] {

                    },
                    model = "project.task",
                    method = "name_search",
                    kwargs = new
                    {
                        name = "",
                        args = new object[][] {
                            new object[] {
                           "project_id","=",projectId
                        },
                        },
                        @operator = "ilike",
                        limit = 100,
                        context = new
                        {
                            default_project_id = projectId,
                            lang = "en_US",
                            tz = "Asia/Ho_Chi_Minh",
                            uid = 65
                        }
                    }

                }
            }); ;
            param.Add("body", body.ToString());

            result = excuteRequest("POST", "https://erp.thgdx.vn/web/dataset/call_kw/project.task/name_search", param);

            return result;
        }
        public JObject createTimeSheet(int projectId, int employee_id, int taskId, string description, string date)
        {
            JObject result = new JObject();
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            JObject body = JObject.FromObject(new
            {
                jsonrpc = "2.0",
                method = "call",
                @params = new
                {
                    args = new object[] {
                       new {
                                date,
                                name= description,
                                unit_amount=8.5,
                                employee_id,
                                project_id= projectId,
                                task_id= taskId
                       }
                    },
                    model = "account.analytic.line",
                    method = "create",
                    kwargs = new
                    {
                        context = new
                        {
                            lang = "en_US",
                            tz = "Asia/Ho_Chi_Minh",
                            uid = 65
                        }
                    }

                }
            }); ;
            param.Add("body", body.ToString());

            result = excuteRequest("POST", "https://erp.thgdx.vn/web/dataset/call_kw/account.analytic.line/create", param);

            return result;
        }
    }
}
