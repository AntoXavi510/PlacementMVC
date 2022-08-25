using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PlacementApplicationNew.Model;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace PlacementMVC.Controllers
{
    public class AdminController : Controller
    {
        string BaseUrl = "https://localhost:44362/";
        [NoDirectAccess]
        public async Task<ActionResult> GetStudents()
        {
            List<Student> StudentsInfo = new List<Student>();
            using (var client = new HttpClient())
            {
                // client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44362/api/Students");
                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    StudentsInfo = JsonConvert.DeserializeObject<List<Student>>(ProdResponse);

                }
                return View(StudentsInfo);
            }

        }
        [NoDirectAccess]

        public async Task<ActionResult> GetRoles()
        {
            List<Role> roleInfo = new List<Role>();
            using (var client = new HttpClient())
            {
                // client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44362/api/Roles");
                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    roleInfo = JsonConvert.DeserializeObject<List<Role>>(ProdResponse);

                }
                return View(roleInfo);
            }

        }
        [NoDirectAccess]

        public async Task<ActionResult> AddCompany()
        {
            return await Task.Run(() => View());
        }
        [HttpPost]
        public async Task<ActionResult> AddCompany(Company company)
        {
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Companies", content);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Success = "The Company is added successfully";
                    return View();
                }
                else
                {
                    ViewBag.Message = "The company is already added please try again";
                    return View();
                }
                return RedirectToAction("Role");
            }
        }

        [NoDirectAccess]

        public async Task<IActionResult> GetCompanies()
        {
            List<Company> CompaniesInfo = new List<Company>();
            using (var client = new HttpClient())
            {
                // client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44362/api/Companies");
                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    CompaniesInfo = JsonConvert.DeserializeObject<List<Company>>(ProdResponse);

                }
                return View(CompaniesInfo);
            }
        }
        [NoDirectAccess]

        public async Task<IActionResult> GetAppliedStudentsListByRole(int id)
        {
            List<Apply> ApplyInfo = new List<Apply>();
            using (var client = new HttpClient())
            {
                // client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44362/api/Applies/GetApplyForRoles?id="+id);
                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    ApplyInfo = JsonConvert.DeserializeObject<List<Apply>>(ProdResponse);

                }
                return View(ApplyInfo);
            }
        }
        [NoDirectAccess]

        public async Task<IActionResult> AddRole()
        {
            List<Company> CompaniesInfo = new List<Company>();
            using (var client = new HttpClient())
            {
                // client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44362/api/Companies");
                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    CompaniesInfo = JsonConvert.DeserializeObject<List<Company>>(ProdResponse);

                }
                var company = CompaniesInfo.Select(s => new { Text = s.CompanyName, Value = s.CompanyId }).ToList();
                ViewBag.Company = new SelectList(company, "Value", "Text");
                return View();

               
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddRole(Role role)
        {
          
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Roles", content);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Success = "The Role is added successfully";
                    return View();
                }

                } 
              return RedirectToAction("GetStudents");
            }

        
        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> EditRole(int Id)
        {
            Role role = new Role();
            using (var client = new HttpClient())
            {
                using (var res = await client.GetAsync("https://localhost:44362/api/Roles/" + Id))
                {
                    string apires = await res.Content.ReadAsStringAsync();
                    role = JsonConvert.DeserializeObject<Role>(apires);
                }
            }
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(Role role)
        {
            using (var client = new HttpClient())
            {
                int id = role.RoleId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");
                using (var res = await client.PutAsync("https://localhost:44362/api/Roles/" + id, content))
                {
                    string apiRes = await res.Content.ReadAsStringAsync();
                    role = JsonConvert.DeserializeObject<Role>(apiRes);
                }
            }
            return RedirectToAction("GetStudents");
        }
        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> RoleDetails(int id)
        {
            Role role = new Role();
            HttpContext.Session.SetInt32("RoleId", id);
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
        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> DeleteCompany(int Id)
        {
            Company company = new Company();
            using (var client = new HttpClient())
            {
                using (var res = await client.GetAsync("https://localhost:44362/api/Companies/" + Id))
                {
                    string apires = await res.Content.ReadAsStringAsync();
                    company = JsonConvert.DeserializeObject<Company>(apires);
                }
            }
            return View(company);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCompany(int Id, Company company)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                await client.DeleteAsync("https://localhost:44362/api/Companies/" + Id);
            }
            return RedirectToAction("GetCompanies");
        }
        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            
            Student student = new Student();
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
        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> DeleteRole(int Id)
        {
            Role role= new Role();  
            using (var client = new HttpClient())
            {
                using (var res = await client.GetAsync("https://localhost:44362/api/Roles/" + Id))
                {
                    string apires = await res.Content.ReadAsStringAsync();
                    role = JsonConvert.DeserializeObject<Role>(apires);
                }
            }
            return View(role);
        }
        

        [HttpPost]
        public async Task<IActionResult> DeleteRole(int Id, Role role)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                await client.DeleteAsync("https://localhost:44362/api/Roles/" + Id);
            }
            return RedirectToAction("GetRoles");
        }
        [NoDirectAccess]

        [HttpGet]
        public async Task<IActionResult> DeleteStudent(int Id)
            {
                Student student = new Student();
                using (var client = new HttpClient())
                {
                    using (var res = await client.GetAsync("https://localhost:44362/api/Students/" + Id))
                    {
                        string apires = await res.Content.ReadAsStringAsync();
                        student = JsonConvert.DeserializeObject<Student>(apires);
                    }
                }
                return View(student);
            }
        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int Id, Student student)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    await client.DeleteAsync("https://localhost:44362/api/Students/" + Id);
                }
                return RedirectToAction("GetStudents");
            }
        [NoDirectAccess]

        public async Task<ActionResult> GetAppliedStudents()
        {
            List<Apply> ApplyInfo = new List<Apply>();
            using (var client = new HttpClient())
            {
                // client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44362/api/Applies");
                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    ApplyInfo = JsonConvert.DeserializeObject<List<Apply>>(ProdResponse);

                }
                return View(ApplyInfo);
            }

        }
        

        public async Task<ActionResult> Login()
        {
            return await Task.Run(() => View());
        }
        [HttpPost]
        public async Task<ActionResult> Login(Admin admin)
        {
            
            HttpContext.Session.SetString("UserName", admin.UserName);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(admin), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44362/api/Admins/Login", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetStudents");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Credentials";
                    return View();
                }
            }
        }
      
        public IActionResult Logout()
        { HttpContext.Session.Clear(); return RedirectToAction("Login", "Admin"); }
    }
    }
    

