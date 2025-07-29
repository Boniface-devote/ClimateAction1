using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class SupabaseConnectionChecker : MonoBehaviour
{
    public Button selectImageButton;
    public Button uploadButton;
    public RawImage previewImage;
    public TextMeshProUGUI statusText;

    private string selectedFilePath;
    private Texture2D selectedTexture;

    private const string SUPABASE_URL = "https://unlbdjmgglhdvgnwhlyj.supabase.co";
    private const string SUPABASE_API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVubGJkam1nZ2xoZHZnbndobHlqIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTM2ODQ2MDUsImV4cCI6MjA2OTI2MDYwNX0.W5nO-hgBQoaWgKNHNnKk1tRAkjew9guPf7pEnoRwfBo"; // Replace with full key
    private const string BUCKET_NAME = "activityimages";
    private const string TABLE_NAME = "player_activities";
    void Start()
    {
        selectImageButton.onClick.AddListener(OpenFileExplorer);
        uploadButton.onClick.AddListener(() => StartCoroutine(UploadActivityImage()));
    }

    void OpenFileExplorer()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        selectedFilePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "jpg,png");

        if (!string.IsNullOrEmpty(selectedFilePath))
        {
            byte[] imageData = File.ReadAllBytes(selectedFilePath);
            selectedTexture = new Texture2D(2, 2);
            selectedTexture.LoadImage(imageData);
            previewImage.texture = selectedTexture;
            statusText.text = "Image selected";
            Debug.Log("Image selected: " + selectedFilePath);
        }
        else
        {
            statusText.text = "No image selected";
            Debug.Log("No image selected");
        }
#endif
    }

    IEnumerator UploadActivityImage()
    {
        if (string.IsNullOrEmpty(selectedFilePath))
        {
            statusText.text = "No image selected.";
            yield break;
        }

        string username = PlayerPrefs.GetString("PlayerName", "anonymous");
        string fileName = $"activity_{username}_{DateTime.UtcNow.Ticks}.jpg";
        string objectPath = $"{BUCKET_NAME}/{fileName}";
        string uploadUrl = $"{SUPABASE_URL}/storage/v1/object/{objectPath}?upsert=true";

        byte[] fileData = File.ReadAllBytes(selectedFilePath);

        UnityWebRequest request = UnityWebRequest.Put(uploadUrl, fileData);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "image/jpeg");
        request.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");
        request.SetRequestHeader("apikey", SUPABASE_API_KEY);

        Debug.Log("Uploading to: " + uploadUrl);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Image upload failed: " + request.error);
            statusText.text = "Image upload failed: " + request.error;
            yield break;
        }

        string publicImageUrl = $"{SUPABASE_URL}/storage/v1/object/public/{objectPath}";
        yield return StartCoroutine(SaveImageDataToTable(username, publicImageUrl));
    }

    IEnumerator SaveImageDataToTable(string username, string imageUrl)
    {
        string tableUrl = $"{SUPABASE_URL}/rest/v1/{TABLE_NAME}";
        string jsonBody = $"{{\"username\":\"{username}\",\"image_url\":\"{imageUrl}\"}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest postRequest = new UnityWebRequest(tableUrl, "POST");
        postRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = new DownloadHandlerBuffer();
        postRequest.SetRequestHeader("Content-Type", "application/json");
        postRequest.SetRequestHeader("apikey", SUPABASE_API_KEY);
        postRequest.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");

        yield return postRequest.SendWebRequest();

        if (postRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Database insert failed: " + postRequest.error);
            statusText.text = "Database insert failed: " + postRequest.error;
        }
        else
        {
            statusText.text = "Activity uploaded successfully!";
            Debug.Log("Activity upload complete.");
        }
    }
}
