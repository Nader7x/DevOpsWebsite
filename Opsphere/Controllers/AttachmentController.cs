using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opsphere.Data.Interfaces;
using Opsphere.Dtos.Attachment;
using Opsphere.Data.Models;
using Opsphere.Mappers;

namespace Opsphere.Controllers;

[Authorize, ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized),
 ProducesResponseType(StatusCodes.Status500InternalServerError), ApiController, Route("Opsphere/Attachment")]
public class AttachmentController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
    : ControllerBase
{
    private readonly IWebHostEnvironment _env = env;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet("AttachementsInfo/{attachmentId:int}")]
    public async Task<IActionResult> GetById([FromRoute] int attachmentId)
    {
        var attachmentModel = await _unitOfWork.AttachmentRepository.GetByIdAsync(attachmentId);

        if (attachmentModel == null)
        {
            return NotFound();
        }

        return Ok(attachmentModel.ToAttachmentDto());
    }

    [HttpPost("{cardId:int}")]
    public async Task<IActionResult> Upload([FromForm] CreateAttachmentDto? createAttachmentDto, [FromRoute] int cardId)
    {
        var card = await _unitOfWork.CardRepository.GetByIdAsync(cardId);
        if (card is not { Status: Status.Done })
        {
            return BadRequest("You can't upload Attachments to UnFinished Card");
        }

        var resourceUrl = $"https://localhost:7157/uploads/{createAttachmentDto?.File?.FileName}";
        if (createAttachmentDto?.File?.FileName == null) return Ok("File uploaded successfully!");
        var filePath = Path.Combine(_env.ContentRootPath, "uploads", createAttachmentDto.File.FileName);
        if (createAttachmentDto.File.Length <= 0) return BadRequest("Failed to upload the file");
        await using var stream = new FileStream(filePath, FileMode.Create);

        var attachment = createAttachmentDto.ToAttachmentFromCreate(cardId);
        attachment.FileUrl = resourceUrl;
        attachment.FilePath = filePath;
        await _unitOfWork.AttachmentRepository.AddAsync(attachment);
        try
        { 
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("This Card Already had an Attachment");
        }

        await createAttachmentDto.File.CopyToAsync(stream);
        return Ok("File uploaded successfully!");
    }

    [HttpGet("CardAttachment/{cardId:int}")]
    public async Task<IActionResult> Serve([FromRoute] int cardId)
    {
        var card = await _unitOfWork.CardRepository.GetByIdAsync(cardId);
        if (card is not { Status: Status.Done }) return BadRequest("No Attachment Found");
        var attachment = await _unitOfWork.AttachmentRepository.GetByCardId(cardId);
        if (attachment == null || string.IsNullOrEmpty(attachment.FilePath)) return BadRequest("No Attachment Found");
        if (!System.IO.File.Exists(attachment.FilePath)) return BadRequest("No Attachment Found");
        var contentType = GetContentType(Path.GetExtension(attachment.FilePath));
        return PhysicalFile(attachment.FilePath, contentType,attachment.FileUrl);
    }

    private static string GetContentType(string fileExtension)
    {
        switch (fileExtension.ToLower())
        {
            case ".txt": return "text/plain";
            case ".pdf": return "application/pdf";
            case ".doc": return "application/msword";
            case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".xls": return "application/vnd.ms-excel";
            case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            case ".ppt": return "application/vnd.ms-powerpoint";
            case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            case ".jpg":
            case ".jpeg": return "image/jpeg";
            case ".png": return "image/png";
            case ".gif": return "image/gif";
            case ".bmp": return "image/bmp";
            case ".tiff":
            case ".tif": return "image/tiff";
            case ".svg": return "image/svg+xml";
            case ".mp3": return "audio/mpeg";
            case ".wav": return "audio/wav";
            case ".ogg": return "audio/ogg";
            case ".flac": return "audio/flac";
            case ".aac": return "audio/aac";
            case ".wma": return "audio/x-ms-wma";
            case ".mp4": return "video/mp4";
            case ".avi": return "video/x-msvideo";
            case ".mkv": return "video/x-matroska";
            case ".webm": return "video/webm";
            case ".mov": return "video/quicktime";
            case ".wmv": return "video/x-ms-wmv";
            case ".flv": return "video/x-flv";
            case ".mpeg":
            case ".mpg": return "video/mpeg";
            case ".zip": return "application/zip";
            case ".rar": return "application/x-rar-compressed";
            case ".7z": return "application/x-7z-compressed";
            case ".tar": return "application/x-tar";
            case ".gz": return "application/gzip";
            case ".bz2": return "application/x-bzip2";
            case ".exe": return "application/octet-stream";
            case ".dll": return "application/octet-stream";
            case ".iso": return "application/octet-stream";
            case ".json": return "application/json";
            case ".xml": return "application/xml";
            case ".html":
            case ".htm": return "text/html";
            case ".css": return "text/css";
            case ".js": return "application/javascript";
            case ".csv": return "text/csv";
            case ".rtf": return "application/rtf";
            // Add more mappings as needed
            default: return "application/octet-stream"; // Default content type for unknown file types
        }
    }
}