using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;

public class TwitchChat : MonoBehaviour
{
    [SerializeField]
    private string username;
    [SerializeField]
    private string password;
    [SerializeField]
    private string channelName;

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    private InvincibilityManager iManager;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.FindGameObjectWithTag("0").GetComponent<PlayerManager>().isInvincible = true;
        Connect();
        //iManager = GameObject.Find("Game Manager").GetComponent<InvincibilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!twitchClient.Connected)
        {
            Connect();
        }
        //TO DO: Try and optimize

        ReadChat();
    }

    private void Connect()
    {

        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();

    }

    private void ReadChat()
    {
        if (twitchClient.Available > 0)
        {
            var message = reader.ReadLine(); //Read in the current message
            print(message);
            if (message.Contains("PRIVMSG"))
            {
                //Get the users name by splitting it from the string
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                //Get the users message by splitting it from the string
                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                print(string.Format("{0}: {1}", chatName, message));
                //chatBox.text = chatBox.text + "\n" + String.Format("{0}: {1}", chatName, message);
                if(message.Equals("b") || message.Equals("B"))
                {
                    InvincibilityManager.Iinstance.increaseBitchCount(ref InvincibilityManager.Iinstance.BCount);
                }
                else if(message.Equals("t") || message.Equals("T"))
                {
                    InvincibilityManager.Iinstance.increaseBitchCount(ref InvincibilityManager.Iinstance.TCount);
                }
                else if(message.Equals("c") || message.Equals("C"))
                {
                    InvincibilityManager.Iinstance.increaseBitchCount(ref InvincibilityManager.Iinstance.CCount);
                }
                else if(message.Equals("h")||message.Equals("H"))
                {
                    InvincibilityManager.Iinstance.increaseBitchCount(ref InvincibilityManager.Iinstance.HCount);
                }
            }
        }
    }
}
