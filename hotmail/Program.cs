using RestSharp;
using System.Collections.Specialized;
using System.Reflection.PortableExecutable;
using HtmlAgilityPack;

async Task<RestResponse> getHtml()
{
    var options = new RestClientOptions("https://login.live.com")
    {
        MaxTimeout = -1,
        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36",
    };
    var client = new RestClient(options);
    var request = new RestRequest("/", Method.Get);
    request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
    request.AddHeader("Accept-Language", "en-US,en;q=0.9");
    request.AddHeader("Connection", "keep-alive");
    request.AddHeader("Sec-Fetch-Dest", "document");
    request.AddHeader("Sec-Fetch-Mode", "navigate");
    request.AddHeader("Sec-Fetch-Site", "none");
    request.AddHeader("Sec-Fetch-User", "?1");
    request.AddHeader("Upgrade-Insecure-Requests", "1");
    request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"112\", \"Google Chrome\";v=\"112\", \"Not:A-Brand\";v=\"99\"");
    request.AddHeader("sec-ch-ua-mobile", "?0");
    request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
  //  request.AddHeader("Cookie", "MSCC=123.16.161.241-VN; MSPOK=$uuid-4f37cc5f-311c-4578-aaaf-af518cc41bd9; MSPRequ=id=N&lt=1683605756&co=1; OParams=11O.DW0xlj1w4cCrJqo5Q8neQm9cSlm9i*LRHYtTKCB6mhGXuBgTq5spVM5AVYwLmK09eKKpmYLxcI9VrcT!3LI82jKzmrWR8sbR*CLk4tGxsW51jYRmGQfR7rwcKGc5L*Qtcw$$; uaid=12ea6bf3829f415d8dcfd150fd4ba271");
    RestResponse response = client.ExecuteAsync(request).GetAwaiter().GetResult();
    return response;
    
}


async Task<string> cookie(RestResponse response)
{
    List<string> setCookies = new List<string> { };
    foreach (var header in response.Headers)
    {
        if (header.ToString().IndexOf("Set-Cookie") != -1)
        {
            setCookies.Add((string)header.Value);
        }
        
    }
    string cookieStr = "";
    int count = 0;
    foreach (string cookie in setCookies)
    {
        string t = ";";
        if (count == setCookies.Count-1)
        {
            t = "";
        }

        cookieStr = cookieStr + cookie.Split(";")[0] + t;            

        count++;
    }
    return cookieStr;
}


async Task<HtmlDocument> DOM(RestResponse response)
{
    if (response != null)
    {
        HtmlDocument document = new HtmlDocument();
        string responseBody = response.Content;
        document.LoadHtml(responseBody);
        return document;
    }
    return null;
}

async Task<string> pathPost(string responseBody)
{
    int posStart ;
    int posEnd;
    string path ="";
    if (responseBody != null)
    {
        posStart = responseBody.IndexOf("https://login.live.com/ppsecure/");
        if(posStart != -1)
        {
            posEnd = responseBody.IndexOf("'", posStart);
            path = responseBody.Substring(posStart +22 , (posEnd - (posStart + 22)));
        }
        
    }
    return path.Trim();
}

async Task<string> FillPPFT(string responseBody)
{
    int posStart;
    int posEnd;
    string temp = "";
    string PPFT = "";
    if (responseBody != null)
    {
        posStart = responseBody.IndexOf("<input type=\"hidden\" name=\"PPFT\" id=\"i0327\" value=\"");
        if (posStart != -1)
        {
            
            posEnd = responseBody.IndexOf("/>", posStart);

            temp = responseBody.Substring(posStart, (posEnd - 1 - posStart ));
            PPFT = temp.Replace("<input type=\"hidden\" name=\"PPFT\" id=\"i0327\" value=\"", "");
        }

    }
    return PPFT.Trim();
}

async Task<RestResponse> postHTMl(string cookies,string PPFT,string path)
{
    var options = new RestClientOptions("https://login.live.com")
    {
        MaxTimeout = -1,
        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36",
    };
    var client = new RestClient(options);
    var request = new RestRequest($"{path}", Method.Post);
    request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
    request.AddHeader("Accept-Language", "en-US,en;q=0.9");
    request.AddHeader("Cache-Control", "max-age=0");
    request.AddHeader("Connection", "keep-alive");
    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
    request.AddHeader("Cookie", cookies);
    request.AddHeader("Origin", "https://login.live.com");
    request.AddHeader("Referer", "https://login.live.com/");
    request.AddHeader("Sec-Fetch-Dest", "document");
    request.AddHeader("Sec-Fetch-Mode", "navigate");
    request.AddHeader("Sec-Fetch-Site", "same-origin");
    request.AddHeader("Sec-Fetch-User", "?1");
    request.AddHeader("Upgrade-Insecure-Requests", "1");
    request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"112\", \"Google Chrome\";v=\"112\", \"Not:A-Brand\";v=\"99\"");
    request.AddHeader("sec-ch-ua-mobile", "?0");
    request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
    request.AddParameter("i13", "0");
    request.AddParameter("login", "thangthang780@gmail.com");
    request.AddParameter("loginfmt", "thangthang780@gmail.com");
    request.AddParameter("type", "11");
    request.AddParameter("LoginOptions", "3");
    request.AddParameter("lrt", "");
    request.AddParameter("lrtPartition", "");
    request.AddParameter("hisRegion", "");
    request.AddParameter("hisScaleUnit", "");
    request.AddParameter("passwd", "Handanba1");
    request.AddParameter("ps", "2");
    request.AddParameter("psRNGCDefaultType", "");
    request.AddParameter("psRNGCEntropy", "");
    request.AddParameter("psRNGCSLK", "");
    request.AddParameter("canary", "");
    request.AddParameter("ctx", "");
    request.AddParameter("hpgrequestid", "");
    request.AddParameter("PPFT",PPFT);
    request.AddParameter("PPSX", "Passp");
    request.AddParameter("NewUser", "1");
    request.AddParameter("FoundMSAs", "");
    request.AddParameter("fspost", "0");
    request.AddParameter("i21", "0");
    request.AddParameter("CookieDisclosure", "0");
    request.AddParameter("IsFidoSupported", "1");
    request.AddParameter("isSignupPost", "0");
    request.AddParameter("isRecoveryAttemptPost", "0");
    request.AddParameter("i19", "1");
    RestResponse response = client.ExecuteAsync(request).GetAwaiter().GetResult();
    
    return response;
}

async Task start()
{
    RestResponse resGet = await getHtml();
    string cookieStr = await cookie(resGet);
    
    string path = await pathPost(resGet.Content);
    string PPFT = await FillPPFT(resGet.Content);
    RestResponse resPost = await postHTMl(cookieStr, PPFT, path);
    Console.WriteLine(resPost.Content);
}

start();