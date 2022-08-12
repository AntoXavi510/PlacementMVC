using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlacementApplicationNew.Model;
using System.Net.Http.Headers;
using System.Text;

namespace PlacementMVC.Controllers
{
    public class AdminController : Controller
    {
        string BaseUrl = "https://localhost:44362/";
      
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
                using (var response = await client.PostAsync("api/Companies", content))
                {
                    string apires = await response.Content.ReadAsStringAsync();
                    company = JsonConvert.DeserializeObject<Company>(apires);
                }
                return RedirectToAction("GetStudents");
            }
        }
        public async Task<ActionResult> GetCompanies()
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
        public async Task<ActionResult> AddRole()
        {
            return await Task.Run(() => View());
        }
        [HttpPost]
        public async Task<ActionResult> AddRole(Role role)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("api/Roles", content))
                {
                    string apires = await response.Content.ReadAsStringAsync();
                    role = JsonConvert.DeserializeObject<Role>(apires);
                }
                return RedirectToAction("GetStudents");
            }
        }
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
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
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
        public async Task<IActionResult> Delete(int Id, Student student)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    await client.DeleteAsync("https://localhost:44362/api/Students/" + Id);
                }
                return RedirectToAction("GetStudents");
            }
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
    }
    }
    

