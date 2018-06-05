using CookComputing.XmlRpc;

namespace BinSend
{
    public static class BitmessageDefaults
    {
        /// <summary>
        /// Default TTL for messages in seconds
        /// </summary>
        /// <remarks>345600s=96h=4d</remarks>
        public const int DEFAULT_TTL = 345600;

        /// <summary>
        /// Default version for address. BM supports 3 and 4
        /// </summary>
        public const int DEFAULT_ADDR_VERSION = 4;
        /// <summary>
        /// Default stream number for addresses. BM supports 1
        /// </summary>
        public const int DEFAULT_ADDR_STREAM = 1;

        /// <summary>
        /// Default type for messages. BM supports 1 and 2
        /// </summary>
        public const int DEFAULT_ENC_TYPE = 2;
    }

    /// <summary>
    /// Decoded Bitmessage Address
    /// </summary>
    public struct BitmessageAddrDecoded
    {
        public string status;
        public int addressVersion;
        public int streamNumber;
        /// <summary>
        /// Base64
        /// </summary>
        public string ripe;
    }

    /// <summary>
    /// Bitmessage address lists
    /// </summary>
    public struct BitmessageAddrInfoContainer
    {
        public BitmessageAddrInfo[] addresses;
    }

    /// <summary>
    /// Bitmessage Address information
    /// </summary>
    public struct BitmessageAddrInfo
    {
        /// <summary>
        /// Base64
        /// </summary>
        public string label;
        public string address;
        public int stream;
        public bool chan;
        public bool enabled;
    }

    /// <summary>
    /// Bitmessage Message
    /// </summary>
    public struct BitmessageMsg
    {
        public string msgid;
        public string toAddress;
        public string fromAddress;
        /// <summary>
        /// Base64
        /// </summary>
        public string subject;
        /// <summary>
        /// Base64
        /// </summary>
        public string message;
        public int encodingType;
        public long receivedTime;
        public bool read;
    }

    /// <summary>
    /// Bitmessage Client Information and Status
    /// </summary>
    public struct BitmessageClientStatus
    {
        public string softwareName;
        public string softwareVersion;
        public BitmessageNetworkStatus networkStatus;
        public int networkConnections;
        public int numberOfPubkeysProcessed;
        public int numberOfMessagesProcessed;
        public int numberOfBroadcastsProcessed;
    }

    /// <summary>
    /// Client message status
    /// </summary>
    public enum BitmessageMessageStatus
    {
        /// <summary>
        /// Message/ID/ackdata not found
        /// </summary>
        notfound,
        /// <summary>
        /// Message is queued for POW
        /// </summary>
        msgqueued,
        /// <summary>
        /// Broadcast is queued for POW
        /// </summary>
        broadcastqueued,
        /// <summary>
        /// Broadcast has been sent
        /// </summary>
        broadcastsent,
        /// <summary>
        /// Client is doing POW for public key request
        /// </summary>
        doingpubkeypow,
        /// <summary>
        /// Client waits for the public key
        /// </summary>
        awaitingpubkey,
        /// <summary>
        /// Client is doing POW for the message
        /// </summary>
        doingmsgpow,
        /// <summary>
        /// This message can't be send (Destination POW requirement too high)
        /// </summary>
        forcepow,
        /// <summary>
        /// Message has been sent but not yet confirmed for delivery
        /// </summary>
        msgsent,
        /// <summary>
        /// Message has been send and no delivery confirmation is expected (DML)
        /// </summary>
        msgsentnoackexpected,
        /// <summary>
        /// Message delivery confirmed
        /// </summary>
        ackreceived
    }

    /// <summary>
    /// Client Network connection status
    /// </summary>
    public enum BitmessageNetworkStatus
    {
        /// <summary>
        /// Client is not connected
        /// </summary>
        /// <remarks>You can still do work for messages and they will wait in the send queue</remarks>
        notConnected,
        /// <summary>
        /// Client is connected but only with outgoing connections
        /// </summary>
        /// <remarks>If this stays for hours
        /// **and you are not using a proxy**,
        /// check your firewall
        /// </remarks>
        connectedButHaveNotReceivedIncomingConnections,
        /// <summary>
        /// Everything OK
        /// </summary>
        connectedAndReceivingIncomingConnections
    }

    /// <summary>
    /// Bitmessage XML RPC interface
    /// </summary>
    /// <remarks>Represents Version 6.3.2</remarks>
    public interface BitmessageRPC : IXmlRpcProxy
    {
        /// <summary>
        /// Concatenates two strings
        /// </summary>
        /// <param name="a">String a</param>
        /// <param name="b">String b</param>
        /// <returns>string.Format("{0}-{1}",a,b);</returns>
        [XmlRpcMethod]
        string helloWorld(string a, string b);

        /// <summary>
        /// Adds two integers
        /// </summary>
        /// <param name="a">Integer a</param>
        /// <param name="b">Integer b</param>
        /// <returns>a+b</returns>
        [XmlRpcMethod]
        int add(int a, int b);

        /// <summary>
        /// Sets the statusbar message
        /// </summary>
        /// <param name="message">Message</param>
        [XmlRpcMethod]
        void statusBar(string message);

        /// <summary>
        /// Lists all addresses
        /// </summary>
        /// <returns>JSON(BitmessageAddrInfo[])</returns>
        [XmlRpcMethod]
        string listAddresses();

        /// <summary>
        /// Creates a random address
        /// </summary>
        /// <param name="label">Base64</param>
        /// <param name="eighteenByteRipe">Short address</param>
        /// <param name="totalDifficulty">Difficulty factor</param>
        /// <param name="smallMessageDifficulty">Difficulty factor for small messages</param>
        /// <returns>Address</returns>
        [XmlRpcMethod]
        string createRandomAddress(string label, bool eighteenByteRipe = false, double totalDifficulty = 1.0, double smallMessageDifficulty = 1.0);

        /// <summary>
        /// Creates a deterministic address
        /// </summary>
        /// <param name="passphrase">Base64</param>
        /// <param name="numberOfAddresses">Number of addresses to generate</param>
        /// <param name="addressVersionNumber">Version number</param>
        /// <param name="streamNumber">Stream number</param>
        /// <param name="eighteenByteRipe">Short address</param>
        /// <param name="totalDifficulty">Difficulty factor</param>
        /// <param name="smallMessageDifficulty">Difficulty factor for small messages</param>
        /// <returns>JSON(string[])</returns>
        [XmlRpcMethod]
        string createDeterministicAddresses(string passphrase, int numberOfAddresses = 1, int addressVersionNumber = 0, int streamNumber = 0, bool eighteenByteRipe = false, double totalDifficulty = 1.0, double smallMessageDifficulty = 1.0);

        /// <summary>
        /// Generates a deterministic address without storing it
        /// </summary>
        /// <param name="passphrase">Base64</param>
        /// <param name="addressVersionNumber">Version number</param>
        /// <param name="streamNumber">Stream number</param>
        /// <returns>Address</returns>
        [XmlRpcMethod]
        string getDeterministicAddress(string passphrase, int addressVersionNumber = BitmessageDefaults.DEFAULT_ADDR_VERSION, int streamNumber = BitmessageDefaults.DEFAULT_ADDR_STREAM);

        /// <summary>
        /// Gets all inbox Messages
        /// </summary>
        /// <returns>JSON(BitmessageMsg[])</returns>
        [XmlRpcMethod]
        string getAllInboxMessages();

        /// <summary>
        /// Gets all message IDs from the inbox
        /// </summary>
        /// <returns>JSON(BitmessageMsg[]), only msgid</returns>
        [XmlRpcMethod]
        string getAllInboxMessageIds();

        /// <summary>
        /// Gets a message from the inbox
        /// </summary>
        /// <param name="msgid">Message ID</param>
        /// <param name="read">Set read flag</param>
        /// <returns>JSON(BitmessageMsg)</returns>
        [XmlRpcMethod]
        string getInboxMessageById(string msgid, bool read = false);

        /// <summary>
        /// Gets a sent message by its ackdata value
        /// </summary>
        /// <param name="ackData">ackdata</param>
        /// <returns>JSON(BitmessageMsg)</returns>
        [XmlRpcMethod]
        string getSentMessageByAckData(string ackData);

        /// <summary>
        /// Gets all sent messages
        /// </summary>
        /// <returns>JSON(BitmessageMsg[])</returns>
        [XmlRpcMethod]
        string getAllSentMessages();

        /// <summary>
        /// Gets a message by its id
        /// </summary>
        /// <param name="msgid">ID</param>
        /// <returns>JSON(BitmessageMsg)</returns>
        [XmlRpcMethod]
        string getSentMessageByID(string msgid);

        /// <summary>
        /// Gets all messages from a specific sender
        /// </summary>
        /// <param name="fromAddress">Sender address</param>
        /// <returns>JSON(BitmessageMsg[])</returns>
        [XmlRpcMethod]
        string getSentMessagesBySender(string fromAddress);

        /// <summary>
        /// Moves a message to trash
        /// </summary>
        /// <param name="msgid">Message ID</param>
        [XmlRpcMethod]
        void trashMessage(string msgid);

        /// <summary>
        /// Sends a Message
        /// </summary>
        /// <param name="toAddress">Receiver</param>
        /// <param name="fromAddress">Sender</param>
        /// <param name="subject">Base64</param>
        /// <param name="message">Base64</param>
        /// <param name="encodingType">Message type</param>
        /// <param name="TTL">TTL in seconds</param>
        /// <returns>ackData</returns>
        [XmlRpcMethod]
        string sendMessage(string toAddress, string fromAddress, string subject, string message, int encodingType = BitmessageDefaults.DEFAULT_ENC_TYPE, int TTL = BitmessageDefaults.DEFAULT_TTL);

        /// <summary>
        /// Sends a Broadcast
        /// </summary>
        /// <param name="fromAddress">Sender</param>
        /// <param name="subject">Base64</param>
        /// <param name="message">Base64</param>
        /// <param name="encodingType">Message type</param>
        /// <param name="TTL">TTL in seconds</param>
        /// <returns>ackData</returns>
        [XmlRpcMethod]
        string sendBroadcast(string fromAddress, string subject, string message, int encodingType = BitmessageDefaults.DEFAULT_ENC_TYPE, int TTL = BitmessageDefaults.DEFAULT_TTL);

        /// <summary>
        /// Gets the status of a Message
        /// </summary>
        /// <param name="ackData">ackData</param>
        /// <returns>MessageStatus</returns>
        [XmlRpcMethod]
        string getStatus(string ackData);

        /// <summary>
        /// Lists all subscribed addresses
        /// </summary>
        /// <returns>JSON(BitmessageAddrInfo[])</returns>
        [XmlRpcMethod]
        string listSubscriptions();

        /// <summary>
        /// Adds a subscription
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="label">Base64</param>
        /// <returns>"Added subscription."</returns>
        [XmlRpcMethod]
        string addSubscription(string address, string label = "");

        /// <summary>
        /// Deletes a subscription
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>"Deleted subscription if it existed."</returns>
        [XmlRpcMethod]
        string deleteSubscription(string address);

        /// <summary>
        /// Gets all entries from the address book
        /// </summary>
        /// <returns>JSON(BitmessageAddrInfo[]) (ignores enabled flag)</returns>
        [XmlRpcMethod]
        string listAddressBookEntries();

        /// <summary>
        /// Adds an entry to the address book
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="label">Base64</param>
        /// <returns>"Added address %s to address book"</returns>
        [XmlRpcMethod]
        string addAddressBookEntry(string address, string label = "");

        /// <summary>
        /// Deletes an address from the address book
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>"Deleted address book entry for %s if it existed"</returns>
        [XmlRpcMethod]
        string deleteAddressBookEntry(string address);

        /// <summary>
        /// Deletes a sent message
        /// </summary>
        /// <param name="ackData">ackData</param>
        /// <returns>"Trashed sent message (assuming message existed)."</returns>
        [XmlRpcMethod]
        string trashSentMessageByAckData(string ackData);

        /// <summary>
        /// Creates a DML address
        /// </summary>
        /// <param name="passphrase">Base64</param>
        /// <returns>string</returns>
        [XmlRpcMethod]
        string createChan(string passphrase);

        /// <summary>
        /// Joins an existing DML
        /// </summary>
        /// <param name="passphrase">Base64</param>
        /// <param name="address">Address</param>
        /// <returns>"success"</returns>
        [XmlRpcMethod]
        string joinChan(string passphrase, string address);

        /// <summary>
        /// Leaves a DML
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>"success"</returns>
        [XmlRpcMethod]
        string leaveChan(string address);

        /// <summary>
        /// Deletes an address
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>"success"</returns>
        [XmlRpcMethod]
        string deleteAddress(string address);

        /// <summary>
        /// Decodes an address
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>JSON(BitmessageAddrDecoded)</returns>
        [XmlRpcMethod]
        string decodeAddress(string address);

        /// <summary>
        /// Gets the client status
        /// </summary>
        /// <returns>BitmessageConnectionStatus</returns>
        [XmlRpcMethod]
        string clientStatus();

        #region Documented but Unimplemented

        //Don't use

        /// <summary>
        /// Adds an address to the black/whitelist
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="label">Base64</param>
        /// <returns>?</returns>
        [XmlRpcMethod]
        string addAddressToBlackWhiteList(string address, string label = "");

        /// <summary>
        /// Deletes an address from the black/whitelist
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>?</returns>
        [XmlRpcMethod]
        string removeAddressFromBlackWhiteList(string address);

        #endregion

        #region UNDOCUMENTED

        //Undocumented but implemented in apy.py (and possibly broken)

        /// <summary>
        /// Sends an already encrypted message
        /// </summary>
        /// <param name="encryptedPayload">HEX</param>
        /// <param name="requiredAverageProofOfWorkNonceTrialsPerByte">Difficulty as integer (1.0=320)</param>
        /// <param name="requiredPayloadLengthExtraBytes">Bytes to add to the payload (1.0=?)</param>
        [XmlRpcMethod]
        void disseminatePreEncryptedMsg(string encryptedPayload, int requiredAverageProofOfWorkNonceTrialsPerByte, int requiredPayloadLengthExtraBytes);

        /// <summary>
        /// Sends a pubkey into the network
        /// </summary>
        /// <param name="payload">HEX</param>
        /// <returns></returns>
        [XmlRpcMethod]
        string disseminatePubkey(string payload);

        /// <summary>
        /// Requests a message by Hash
        /// </summary>
        /// <param name="requestedHash">HEX</param>
        /// <returns>JSON(BitmessageMsg)</returns>
        [XmlRpcMethod]
        string getMessageDataByDestinationHash(string requestedHash);

        /// <summary>
        /// Identical to <see cref="getMessageDataByDestinationHash(string)"/> 
        /// </summary>
        /// <param name="requestedHash">Identical to <see cref="getMessageDataByDestinationHash(string)"/></param>
        /// <returns>Identical to <see cref="getMessageDataByDestinationHash(string)"/></returns>
        [XmlRpcMethod]
        string getMessageDataByDestinationTag(string requestedHash);

        /// <summary>
        /// Deletes all trashed messages and vacuums the database
        /// </summary>
        /// <returns>"done"</returns>
        [XmlRpcMethod]
        string deleteAndVacuum();

        /// <summary>
        /// Shuts the client down
        /// </summary>
        /// <returns>"done"</returns>
        [XmlRpcMethod]
        string shutdown();

        #endregion
    }
}
