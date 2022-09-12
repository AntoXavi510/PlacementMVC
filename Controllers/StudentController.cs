using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using PlacementApplicationNew.Model;
using PlacementApplicationNew.Token;
using System.Net.Http.Headers;
using System.Text;
namespace PlacementMVC.Controllers
{
    public class StudentController : Controller
    {
        string BaseUrl = "https://localhost:44362/"; 
        public async Task<ActionResult> Login()
        {
            return await Task.Run(() => View());
        }
        //[HttpPost]
        //public async Task<ActionResult> Login(Student student)
        //{
        //    Student s1 = new Student();
        //    student.CPassword = student.Password;
        //    HttpContext.Session.SetInt32("StudentId", student.UserId);

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(BaseUrl);
        //        StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync("https://localhost:44362/api/Students/Login", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string apires = await response.Content.ReadAsStringAsync();
        //            s1 = JsonConvert.DeserializeObject<Student>(apires);
        //            HttpContext.Session.SetString("FirstName", s1.FirstName);
        //            return RedirectToAction("Role");
        //        }
        //        else
        //        {
        //            ViewBag.ErrorMessage="Invalid Credentials";
        //            return View();
        //        }               
        //    }

        //}
        [HttpPost]
        public async Task<IActionResult> Login(Student? student)
        {
            StudentToken? mt = new StudentToken();

            using (HttpClient httpClient = new HttpClient())
            {



                student.CPassword = student.Password;
                httpClient.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:44362/api/Students/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    var StudentResponse = response.Content.ReadAsStringAsync().Result;
                    mt = JsonConvert.DeserializeObject<StudentToken>(StudentResponse);
                    if (mt == null)
                    {
                        ViewBag.ErrorMessage = "Invalid Credentials";
                        return View();
                    }
                    TempData["StudentId"] = mt.student.UserId;
                    HttpContext.Session.SetInt32("StudentId", mt.student.UserId);
                    HttpContext.Session.SetString("FirstName", mt.student.FirstName);




                    string token = mt.studentToken;
                    HttpContext.Session.SetString("token", token);



                    return RedirectToAction("Role");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Credentials";
                    return View();
                }
            }

    }

    [NoDirectAccess]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
           id = (int)HttpContext.Session.GetInt32("StudentId");
            Student student = new Student();
            if (id != null)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        using (var Res = await client.GetAsync("https://localhost:44362/api/Students/" + id))
                        {
                            string apires = await Res.Content.ReadAsStringAsync();
                            student = JsonConvert.DeserializeObject<Student>(apires);
                            
                        }
                    }
                    return View(student);
                }

                catch (Exception sqlEx) { ViewBag.ErrorMsg = sqlEx.Message; }
            }


             return RedirectToAction("login", "Student");
                
            
            
        }
       


        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {   id = (int)HttpContext.Session.GetInt32("StudentId");
            Student student = new Student();
            student.CPassword = student.Password;
            using (var client = new HttpClient())
            {
                using (var res = await client.GetAsync("https://localhost:44362/api/Students/" + id))
                {
                    string apires = await res.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<Student>(apires);
                }
            }
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            TempData["Message"] = "Updated Successfully";
            using (var client = new HttpClient())
            {
                int id = student.UserId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                using (var res = await client.PutAsync("https://localhost:44362/api/Students/" + id, content))
                {
                    string apiRes = await res.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<Student>(apiRes);
                }
            }
            return RedirectToAction("Role");
        }
        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> DeleteAppliedDetails(int Id)
        {
            Apply apply = new Apply();
            using (var client = new HttpClient())
            {
                using (var res = await client.GetAsync("https://localhost:44362/api/Applies/" + Id))
                {
                    string apires = await res.Content.ReadAsStringAsync();
                    apply = JsonConvert.DeserializeObject<Apply>(apires);
                }
            }
            return View(apply);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAppliedDetails(int Id, Apply apply)
        {
            TempData["Delete"] = "Deleted Successfully";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                await client.DeleteAsync("https://localhost:44362/api/Applies/" + Id);
            }

            return RedirectToAction("AppliedDetails");
        }
        [NoDirectAccess]
        public async Task<ActionResult> Create()
        {
            return await Task.Run(() => View());
        }
        [HttpPost]
        public async Task<ActionResult> Create(Student student)
        { 
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44362/api/Students", content);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Msg = "User Successfully Registered";
                    return View();
                }
                else
                {
                    ViewBag.Message = "User Id Already Found";
                    return View();
                }
            }
        }
        [NoDirectAccess]
        public async Task<ActionResult> Apply()
        {
            return await Task.Run(() => View());
        }
        [HttpPost]
        public async Task<ActionResult> Apply(Apply apply)
        {
            Apply applydetails=new Apply();
            using (var client = new HttpClient())
            {
                var id= HttpContext.Session.GetInt32("RoleId");
                ViewBag.RoleId = id;
                apply.RoleId=@ViewBag.RoleId;
                var StudentId = HttpContext.Session.GetInt32("StudentId");
                ViewBag.StudentId = StudentId;
                apply.StudentId = @ViewBag.StudentId;
                string? token = HttpContext.Session.GetString("token");
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(apply), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/Applies", content);
                if (response.IsSuccessStatusCode)
                {
                    string apires = await response.Content.ReadAsStringAsync();
                    applydetails = JsonConvert.DeserializeObject<Apply>(apires);

                    ViewBag.Success = "You have successfully applied for the role with Role Id:"+@ViewBag.RoleId;
                    return View();
                }
                else
                {
                    ViewBag.Message = "The user is already registered or insufficient CGPA";
                    return View();
                }
                return RedirectToAction("Role");
            }
        }
        [NoDirectAccess]

        public async Task<ActionResult> Role()
        {
            ViewBag.StudentId=HttpContext.Session.GetInt32("StudentId");
            ViewBag.StudentName = HttpContext.Session.GetString("FirstName");
            List<Role> RoleInfo = new List<Role>();
            using (var client = new HttpClient())
            {
                // client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44362/api/Roles");
                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    RoleInfo = JsonConvert.DeserializeObject<List<Role>>(ProdResponse);

                }
                return View(RoleInfo);
            }
        }
        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> AppliedDetails()
        {
            List<Apply> apply = new List<Apply>();
            
            using (var client = new HttpClient())
            {
                // client.BaseAddress = new Uri(Baseurl);
                var StudentId = HttpContext.Session.GetInt32("StudentId");
                ViewBag.StudentId = StudentId;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44362/api/Applies/GetRolesForStudent?id="+ViewBag.StudentId);
                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    apply= JsonConvert.DeserializeObject<List<Apply>>(ProdResponse);

                }
                return View(apply);
            }
        }
        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> RoleDetails(int id)
        {
            Role role = new Role();
            HttpContext.Session.SetInt32("RoleId",id);
            using (var client = new HttpClient())
            {
                using (var Res = await client.GetAsync("https://localhost:44362/api/Roles/" + id))
                {
                    string apires = await Res.Content.ReadAsStringAsync();
                    role = JsonConvert.DeserializeObject<Role>(apires);
                }
            }
            return View(role);

        }
        public IActionResult Logout()
        { HttpContext.Session.Clear(); return RedirectToAction("Login", "Student"); }
    }
}
