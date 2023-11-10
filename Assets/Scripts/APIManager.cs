using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using OpenAI.Chat;
using System.Threading.Tasks;

/// <summary>
/// Public class that represents an API manager for interacting with APIs such as the OpenAI API
/// </summary>
public static class APIManager
{
    /// <summary>
    /// Public static async method that sends an API request to GPT3.5 and waits for a response
    /// </summary>
    /// <param name="userMessage">User-defined message to send as part of the request</param>
    /// <returns>String that represents the response from GPT3.5</returns>
    public static async Task<string> makeGPT3_5Request(string userMessage)
    {
        var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());

        var messages = new List<Message>()
        {
            new Message(Role.System, "Your role is to help simulate a 2D edutainment game focused on New Zealand politics"),
            new Message(Role.User, userMessage),
        };

        var chatRequest = new ChatRequest(messages, model: "gpt-3.5-turbo-1106");
        var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

        return result.FirstChoice.Message.Content;
    }
}
