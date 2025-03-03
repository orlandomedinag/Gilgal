using GilgalInventar.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GilgalInventar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        private readonly gilgalContext _context;
        static string DocxConvertedToHtmlDirectory = @"doccargados\";
        private static string workDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "doccargados");
        private IHostingEnvironment hostingEnv;

        public ArchivosController(gilgalContext context)
        {
            _context = context;
        }

        //GetArchivo
        [HttpGet("{nombre}", Name = "GetArchivo")]
        public async Task<object> GetArchivo(string nombre)
        {
            var archivo = await _context.Archivos.OrderByDescending(f => f.Creado).FirstOrDefaultAsync(f => f.Nombre == nombre);
            if (archivo != null)
            {
                return new JsonResult(archivo);
            }
            else
            {
                return new JsonResult(new Archivo());
            }
        }


        [HttpPost("[action]")]
        public IActionResult save(IFormFile file)
        {
            var sourcefile = workDir;
            try
            {
                if (file != null && file.Length > 0)
                {
                    // Extraemos el contenido en Bytes del archivo subido.
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        // Separamos el Nombre del archivo de la Extensión.
                        int indiceDelUltimoPunto = file.FileName.LastIndexOf('.');
                        string _nombre = file.FileName.Substring(0, indiceDelUltimoPunto);
                        string _extension = file.FileName.Substring(indiceDelUltimoPunto + 1,
                                            file.FileName.Length - indiceDelUltimoPunto - 1);
                        Archivo _archivo = new Archivo()
                        {
                            Nombre = file.FileName,
                            Extension = _extension,
                            Descargas = 0,
                            Contenido = memoryStream.ToArray(),
                            Mime = GetMimeTypes()[_extension]
                        };

                        // Subimos el archivo al Servidor.
                        _archivo.SubirArchivo(_archivo.Contenido);
                        // Instanciamos la clase Archivo y asignammos los valores.
                        var htmlFileName = _archivo.Id.ToString() + ".html";
                        var pdfFileName = _archivo.Id.ToString() + ".pdf";
                        string fileViewer = string.Empty;
                        switch (_extension)
                        {
                            case "docx":
                            case "doc":
                                //fileViewer = htmlFileName;
                                //_archivo.ExtensionViewer = "html";
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "xlsx":
                            case "xls":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pptx":
                            case "ppt":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pdf":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "jpg":
                            case "png":
                            case "gif":
                            case "bmp":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            default:
                                fileViewer = _archivo.Nombre;
                                _archivo.ExtensionViewer = _extension;
                                break;
                        }
                        //_archivo.PathViewer = fileViewer;
                        _archivo.PathViewer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", DocxConvertedToHtmlDirectory, fileViewer);
                        _archivo.PathVista = Path.Combine(@"/", DocxConvertedToHtmlDirectory, fileViewer).Replace(@"\", @"/");
                        if (_archivo.ExtensionViewer == "pdf")
                            CargarArchivoView(_archivo.Id, _archivo.PathCompleto, _archivo.Extension);
                        sourcefile = Path.Combine(workDir, fileViewer);
                        _context.Archivos.Add(_archivo);
                        _context.SaveChanges();
                        return Ok(_archivo);
                    }
                } else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Fallo al subir el archivo";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
                return BadRequest(Response);
            }

        }


        [HttpPost("[action]")]
        public IActionResult savef1(IFormFile file1)
        {
            var sourcefile = workDir;
            try
            {
                if (file1 != null && file1.Length > 0)
                {
                    // Extraemos el contenido en Bytes del archivo subido.
                    using (var memoryStream = new MemoryStream())
                    {
                        file1.CopyTo(memoryStream);
                        // Separamos el Nombre del archivo de la Extensión.
                        int indiceDelUltimoPunto = file1.FileName.LastIndexOf('.');
                        string _nombre = file1.FileName.Substring(0, indiceDelUltimoPunto);
                        string _extension = file1.FileName.Substring(indiceDelUltimoPunto + 1,
                                            file1.FileName.Length - indiceDelUltimoPunto - 1);
                        Archivo _archivo = new Archivo()
                        {
                            Nombre = file1.FileName,
                            Extension = _extension,
                            Descargas = 0,
                            Contenido = memoryStream.ToArray(),
                            Mime = GetMimeTypes()[_extension]
                        };

                        // Subimos el archivo al Servidor.
                        _archivo.SubirArchivo(_archivo.Contenido);
                        // Instanciamos la clase Archivo y asignammos los valores.
                        var htmlFileName = _archivo.Id.ToString() + ".html";
                        var pdfFileName = _archivo.Id.ToString() + ".pdf";
                        string fileViewer = string.Empty;
                        switch (_extension)
                        {
                            case "docx":
                            case "doc":
                                //fileViewer = htmlFileName;
                                //_archivo.ExtensionViewer = "html";
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "xlsx":
                            case "xls":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pptx":
                            case "ppt":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pdf":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "jpg":
                            case "png":
                            case "gif":
                            case "bmp":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            default:
                                fileViewer = _archivo.Nombre;
                                _archivo.ExtensionViewer = _extension;
                                break;
                        }
                        //_archivo.PathViewer = fileViewer;
                        _archivo.PathViewer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", DocxConvertedToHtmlDirectory, fileViewer);
                        _archivo.PathVista = Path.Combine(@"/", DocxConvertedToHtmlDirectory, fileViewer).Replace(@"\", @"/");
                        //if (_archivo.ExtensionViewer == "pdf")
                        //    CargarArchivoView(_archivo.Id, _archivo.PathCompleto, _archivo.Extension);
                        sourcefile = Path.Combine(workDir, fileViewer);
                        _context.Archivos.Add(_archivo);
                        _context.SaveChanges();
                        return Ok(_archivo);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Fallo al subir el archivo";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
                return BadRequest(Response);
            }
        }


        [HttpPost("[action]")]
        public IActionResult savef2(IFormFile file2)
        {
            var sourcefile = workDir;
            try
            {
                if (file2 != null && file2.Length > 0)
                {
                    // Extraemos el contenido en Bytes del archivo subido.
                    using (var memoryStream = new MemoryStream())
                    {
                        file2.CopyTo(memoryStream);
                        // Separamos el Nombre del archivo de la Extensión.
                        int indiceDelUltimoPunto = file2.FileName.LastIndexOf('.');
                        string _nombre = file2.FileName.Substring(0, indiceDelUltimoPunto);
                        string _extension = file2.FileName.Substring(indiceDelUltimoPunto + 1,
                                            file2.FileName.Length - indiceDelUltimoPunto - 1);
                        Archivo _archivo = new Archivo()
                        {
                            Nombre = file2.FileName,
                            Extension = _extension,
                            Descargas = 0,
                            Contenido = memoryStream.ToArray(),
                            Mime = GetMimeTypes()[_extension]
                        };

                        // Subimos el archivo al Servidor.
                        _archivo.SubirArchivo(_archivo.Contenido);
                        // Instanciamos la clase Archivo y asignammos los valores.
                        var htmlFileName = _archivo.Id.ToString() + ".html";
                        var pdfFileName = _archivo.Id.ToString() + ".pdf";
                        string fileViewer = string.Empty;
                        switch (_extension)
                        {
                            case "docx":
                            case "doc":
                                //fileViewer = htmlFileName;
                                //_archivo.ExtensionViewer = "html";
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "xlsx":
                            case "xls":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pptx":
                            case "ppt":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pdf":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "jpg":
                            case "png":
                            case "gif":
                            case "bmp":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            default:
                                fileViewer = _archivo.Nombre;
                                _archivo.ExtensionViewer = _extension;
                                break;
                        }
                        //_archivo.PathViewer = fileViewer;
                        _archivo.PathViewer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", DocxConvertedToHtmlDirectory, fileViewer);
                        _archivo.PathVista = Path.Combine(@"/", DocxConvertedToHtmlDirectory, fileViewer).Replace(@"\", @"/");
                        if (_archivo.ExtensionViewer == "pdf")
                            CargarArchivoView(_archivo.Id, _archivo.PathCompleto, _archivo.Extension);
                        sourcefile = Path.Combine(workDir, fileViewer);
                        _context.Archivos.Add(_archivo);
                        _context.SaveChanges();
                        return Ok(_archivo);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Fallo al subir el archivo";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
                return BadRequest(Response);
            }
        }


        [HttpPost("[action]")]
        public IActionResult savef3(IFormFile file3)
        {
            var sourcefile = workDir;
            try
            {
                if (file3 != null && file3.Length > 0)
                {
                    // Extraemos el contenido en Bytes del archivo subido.
                    using (var memoryStream = new MemoryStream())
                    {
                        file3.CopyTo(memoryStream);
                        // Separamos el Nombre del archivo de la Extensión.
                        int indiceDelUltimoPunto = file3.FileName.LastIndexOf('.');
                        string _nombre = file3.FileName.Substring(0, indiceDelUltimoPunto);
                        string _extension = file3.FileName.Substring(indiceDelUltimoPunto + 1,
                                            file3.FileName.Length - indiceDelUltimoPunto - 1);
                        Archivo _archivo = new Archivo()
                        {
                            Nombre = file3.FileName,
                            Extension = _extension,
                            Descargas = 0,
                            Contenido = memoryStream.ToArray(),
                            Mime = GetMimeTypes()[_extension]
                        };

                        // Subimos el archivo al Servidor.
                        _archivo.SubirArchivo(_archivo.Contenido);
                        // Instanciamos la clase Archivo y asignammos los valores.
                        var htmlFileName = _archivo.Id.ToString() + ".html";
                        var pdfFileName = _archivo.Id.ToString() + ".pdf";
                        string fileViewer = string.Empty;
                        switch (_extension)
                        {
                            case "docx":
                            case "doc":
                                //fileViewer = htmlFileName;
                                //_archivo.ExtensionViewer = "html";
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "xlsx":
                            case "xls":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pptx":
                            case "ppt":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pdf":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "jpg":
                            case "png":
                            case "gif":
                            case "bmp":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            default:
                                fileViewer = _archivo.Nombre;
                                _archivo.ExtensionViewer = _extension;
                                break;
                        }
                        //_archivo.PathViewer = fileViewer;
                        _archivo.PathViewer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", DocxConvertedToHtmlDirectory, fileViewer);
                        _archivo.PathVista = Path.Combine(@"/", DocxConvertedToHtmlDirectory, fileViewer).Replace(@"\", @"/");
                        if (_archivo.ExtensionViewer == "pdf")
                            CargarArchivoView(_archivo.Id, _archivo.PathCompleto, _archivo.Extension);
                        sourcefile = Path.Combine(workDir, fileViewer);
                        _context.Archivos.Add(_archivo);
                        _context.SaveChanges();
                        return Ok(_archivo);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Fallo al subir el archivo";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
                return BadRequest(Response);
            }
        }

        [HttpPost("[action]")]
        public IActionResult saveimg(IFormFile img)
        {
            var sourcefile = workDir;
            try
            {
                if (img != null && img.Length > 0)
                {
                    // Extraemos el contenido en Bytes del archivo subido.
                    using (var memoryStream = new MemoryStream())
                    {
                        img.CopyTo(memoryStream);
                        // Separamos el Nombre del archivo de la Extensión.
                        int indiceDelUltimoPunto = img.FileName.LastIndexOf('.');
                        string _nombre = img.FileName.Substring(0, indiceDelUltimoPunto);
                        string _extension = img.FileName.Substring(indiceDelUltimoPunto + 1,
                                            img.FileName.Length - indiceDelUltimoPunto - 1);
                        Archivo _archivo = new Archivo()
                        {
                            Nombre = img.FileName,
                            Extension = _extension,
                            Descargas = 0,
                            Contenido = memoryStream.ToArray(),
                            Mime = GetMimeTypes()[_extension]
                        };

                        // Subimos el archivo al Servidor.
                        _archivo.SubirArchivo(_archivo.Contenido);
                        // Instanciamos la clase Archivo y asignammos los valores.
                        var htmlFileName = _archivo.Id.ToString() + ".html";
                        var pdfFileName = _archivo.Id.ToString() + ".pdf";
                        string fileViewer = string.Empty;
                        switch (_extension)
                        {
                            case "docx":
                            case "doc":
                                //fileViewer = htmlFileName;
                                //_archivo.ExtensionViewer = "html";
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "xlsx":
                            case "xls":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pptx":
                            case "ppt":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "pdf":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            case "jpg":
                            case "png":
                            case "gif":
                            case "bmp":
                                fileViewer = pdfFileName;
                                _archivo.ExtensionViewer = "pdf";
                                break;
                            default:
                                fileViewer = _archivo.Nombre;
                                _archivo.ExtensionViewer = _extension;
                                break;
                        }
                        //_archivo.PathViewer = fileViewer;
                        _archivo.PathViewer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", DocxConvertedToHtmlDirectory, fileViewer);
                        _archivo.PathVista = Path.Combine(@"/", DocxConvertedToHtmlDirectory, fileViewer).Replace(@"\", @"/");
                        //if (_archivo.ExtensionViewer == "pdf")
                            //CargarArchivoView(_archivo.Id, _archivo.PathCompleto, _archivo.Extension);
                        sourcefile = Path.Combine(workDir, fileViewer);
                        _context.Archivos.Add(_archivo);
                        _context.SaveChanges();
                        return Ok(_archivo);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Fallo al subir el archivo";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
                return BadRequest(Response);
            }
        }


        [HttpPost("[action]")]
        public void remove(IFormFile UploadFile)
        {
            try
            {
                var filename = hostingEnv.ContentRootPath + $@"\{UploadFile.FileName}";
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "El archivo fué removido exitosamente";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        public void CargarArchivoView(Guid file, string filesource, string extension)
        {
            //string filesource = file.ToString();
            var archivo = System.IO.File.ReadAllBytes(filesource);
            var _PathAplicacion = Directory.GetCurrentDirectory();
            string PathCompleto = Path.Combine(_PathAplicacion, "wwwroot", DocxConvertedToHtmlDirectory);
            DirectoryInfo convertedDocsDirectory = new DirectoryInfo(PathCompleto);
            if (!convertedDocsDirectory.Exists)
                convertedDocsDirectory.Create();
            //Guid g = Guid.NewGuid();
            var htmlFileName = file.ToString() + ".html";
            var pdfFileName = file.ToString() + ".pdf";
            switch (extension)
            {
                case "docx":
                case "doc":
                case "pptx":
                case "ppt":
                    //ConvertToHtml(archivo, convertedDocsDirectory, htmlFileName);
                    ConvertDocToPdf(filesource, convertedDocsDirectory, pdfFileName);
                    break;
                case "xlsx":
                case "xls":
                    //ConvertXlsToPdf(filesource, convertedDocsDirectory, pdfFileName);
                    ConvertDocToPdf(filesource, convertedDocsDirectory, pdfFileName);
                    break;
                case "jpg":
                case "png":
                case "gif":
                case "bmp":
                    ConvertDocToPdf(filesource, convertedDocsDirectory, pdfFileName);
                    break;
                case "pdf":
                    //System.IO.File.Copy(filesource, Path.Combine(PathCompleto, pdfFileName), true);
                    break;
                default:
                    Console.WriteLine("Default case");
                    //ConvertDocToPdf(filesource, convertedDocsDirectory, pdfFileName);
                    break;
            }
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
            {"txt", "text/plain"},
            {"pdf", "application/pdf"},
            {"doc", "application/msword"},
            {"docx", "application/vnd.ms-word"},
            {"png", "image/png"},
            {"jpg", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"bmp", "image/bmp"},
            {"gif", "image/gif"},
            {"avi", "video/x-msvideo"},
            {"mp4", "video/mp4"},
            {"htm", "text/html"},
            {"html", "text/html"},
            {"cshtml", "text/html"},
            {"xls", "application/vnd.ms-excel"},
            {"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {"pps", "application/vnd.ms-powerpoint"},
            {"ppt", "application/vnd.ms-powerpoint"},
            {"pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"}
            };
        }

        public void ConvertDocToPdf(string filesource, DirectoryInfo destDirectory, string pdfFileName)
        {
            var pdfProcess = new Process();
            //Modificaciones para garantizar multiplataforma
            string PathCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LibreOfficePortablePrevious\\App\\libreoffice\\program\\soffice.exe");
            //string PathCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _ejecutableLO);
            //string PathCompleto = Path.Combine("@",_ejecutableLO);
            //string workDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "doccargados\\");
            workDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "doccargados");
            string outFile = Path.Combine(destDirectory.FullName, pdfFileName);
            string fileConvert = Path.Combine(workDir, pdfFileName);
            pdfProcess.StartInfo.FileName = PathCompleto;
            pdfProcess.StartInfo.Arguments = String.Format("--norestore --nofirststartwizard --headless --convert-to pdf  \"{0}\""
                                      , filesource);
            pdfProcess.StartInfo.WorkingDirectory = workDir;
            pdfProcess.StartInfo.RedirectStandardOutput = true;
            pdfProcess.StartInfo.RedirectStandardError = true;
            pdfProcess.StartInfo.UseShellExecute = false;
            pdfProcess.Start();
            while (!pdfProcess.HasExited && pdfProcess.Responding)
            {
                Thread.Sleep(100);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Archivo remision)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Archivos.Add(remision);
                    await _context.SaveChangesAsync();
                    return Ok();
                } else
                {
                    return BadRequest();
                }
                //remision.Conceptos = string.Join(",", remision.lstConceptos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // PUT api/<ValuesController1>/5
        [HttpPut]
        public async Task<IActionResult> Put(long id, [FromBody] Archivo remision)
        {
            try
            {
                _context.Entry(remision).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // DELETE api/<ValuesController1>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var remision = await _context.Archivos.FindAsync(id);
                if (remision != null)
                {
                    //_context.Remove(remision);
                    _context.Entry(remision).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
