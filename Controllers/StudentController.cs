using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlacementApplicationNew.Model;
using System.Net.Http.Headers;
using System.Text;
namespace PlacementMVC.Controllers
{
    public class StudentController : Controller
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
        public async Task<ActionResult> Login()
        {
            return await Task.Run(() => View());
        }
        [HttpPost]
        public async Task<ActionResult> Login(Student student)
        {
            HttpContext.Session.SetInt32("StudentId",student.UserId);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("api/Students/Login", content))
                {
                    string apires = await response.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<Student>(apires);
                }
                return RedirectToAction("Role");
            }
        }
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
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
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
        public async Task<IActionResult> Edit(Student student)
        {

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
            return RedirectToAction("GetStudents");
        }
        public async Task<ActionResult> Create()
        {
            return await Task.Run(() => View());
        }
        [HttpPost]
        public async Task<ActionResult> Create(Student student)
        {
            Student students = new Student();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("api/Students", content))
                {
                    string apires = await response.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<Student>(apires);
                }
                return RedirectToAction("GetStudents");
            }
        }
        public async Task<ActionResult> Apply()
        {
            return await Task.Run(() => View());
        }
        [HttpPost]
        public async Task<ActionResult> Apply(Apply apply)
        {
            
            using (var client = new HttpClient())
            {
                var id= HttpContext.Session.GetInt32("RoleId");
                ViewBag.RoleId = id;
                apply.RoleId=@ViewBag.RoleId;
                var StudentId = HttpContext.Session.GetInt32("StudentId");
                ViewBag.StudentId = StudentId;
                apply.StudentId = @ViewBag.StudentId;

                client.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(apply), Encoding.UTF8, "application/json");
                
                using (var response = await client.PostAsync("api/Applies", content))
                {
                    string apires = await response.Content.ReadAsStringAsync();
                    apply = JsonConvert.DeserializeObject<Apply>(apires);
                }
                return RedirectToAction("Role");
            }
        }
        public async Task<ActionResult> Role()
        {
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
