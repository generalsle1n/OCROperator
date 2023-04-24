using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OCROperator.Models
{
    public class PapercutItem
    {
        [JsonPropertyName("version")]
        public string Version { get; init; }
        [JsonPropertyName("accountName")]
        public string AccountName { get; init; }
        [JsonPropertyName("date")]
        public string Date { get; init; }
        [JsonPropertyName("deviceName")]
        public string DeviceName { get; init; }
        [JsonPropertyName("type")]
        public string Type { get; init; }
        [JsonPropertyName("fields")]
        public Field[] Fields { get; init; }
        [JsonPropertyName("files")]
        public string[] Files { get; init; }
        [JsonPropertyName("jobId")]
        public string JobID { get; init; }
        [JsonPropertyName("name")]
        public string Name { get; init; }
        [JsonPropertyName("settings")]
        public Setting Setting { get; init; }
        [JsonPropertyName("user")]
        public User User { get; init; }

        private const string ScanToFolderIDPath = "action.folder_any.";

        public string GetPathWithFile()
        {
            string Result = string.Empty;
            foreach(Field Setting in Fields)
            {
                if (Setting.ID.StartsWith(ScanToFolderIDPath))
                {
                    Result = $@"{Setting.Value}\{Files[0]}";
                }
            }
            return Result;
        }
    }

    public class Field
    {
        [JsonPropertyName("id")]
        public string ID { get; init; }
        [JsonPropertyName("label")]
        public string Label { get; init; }
        [JsonPropertyName("value")]
        public string Value { get; init; }
    }
    public class Setting
    {
        [JsonPropertyName("fileType")]
        public string FileType { get; init; }
        [JsonPropertyName("ocrEnabled")]
        public bool OCREnabled { get; init; }
        [JsonPropertyName("splitOnBlank")]
        public bool SplitOnBlank { get; init; }
        [JsonPropertyName("splitByPage")]
        public bool SplitByPage { get; init; }
        [JsonPropertyName("splitBySize")]
        public string SplitBySize { get; init; }
        [JsonPropertyName("despeckle")]
        public bool Despeckle { get; init; }
        [JsonPropertyName("deskew")]
        public bool Deskew { get; init; }
        [JsonPropertyName("pdfaEnabled")]
        public bool PdfaEnabled { get; init; }
    }
    public class User
    {
        [JsonPropertyName("department")]
        public string Department { get; init; }
        [JsonPropertyName("email")]
        public string Email { get; init; }
        [JsonPropertyName("groups")]
        public string[] Groups { get; init; }
        [JsonPropertyName("name")]
        public string Name { get; init; }
        [JsonPropertyName("office")]
        public string Office { get; init; }

    }
}