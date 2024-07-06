import 'package:flutter/material.dart';
import 'package:cloud_firestore/cloud_firestore.dart';

class ChatPage extends StatefulWidget {
  final String senderId;
  final String receiverId;
  final String reciverName;
  final String reciverAgency;

  const ChatPage({super.key, required this.senderId, required this.receiverId, required this.reciverName, required this.reciverAgency});

  @override
  _ChatPage createState() => _ChatPage();
}

class _ChatPage extends State<ChatPage> {
  final TextEditingController _controller = TextEditingController();

  void _sendMessage() {
    if (_controller.text.isEmpty) {
      return;
    }
    FirebaseFirestore.instance.collection('chats').add({
      'text': _controller.text,
      'senderId': widget.senderId,
      'receiverId': widget.receiverId,
      'timestamp': FieldValue.serverTimestamp(),
    });
    _controller.clear();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.white,
        title: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(widget.reciverName),
            SizedBox(height: 5),
            Text(widget.reciverAgency, style: TextStyle(fontSize: 10)),
          ],
        ),
      ),
      body: Column(
        children: [
          Expanded(
            child: StreamBuilder(
              stream: FirebaseFirestore.instance
                  .collection('chats')
                  .where('senderId', isEqualTo: widget.senderId)
                  .where('receiverId', isEqualTo: widget.receiverId)
                  .orderBy('timestamp', descending: true)
                  .snapshots(),
              builder: (ctx, AsyncSnapshot<QuerySnapshot> sentSnapshot) {
                if (sentSnapshot.connectionState == ConnectionState.waiting) {
                  return Center(child: CircularProgressIndicator());
                }
                return StreamBuilder(
                  stream: FirebaseFirestore.instance
                      .collection('chats')
                      .where('senderId', isEqualTo: widget.receiverId)
                      .where('receiverId', isEqualTo: widget.senderId)
                      .orderBy('timestamp', descending: true)
                      .snapshots(),
                  builder:
                      (ctx, AsyncSnapshot<QuerySnapshot> receivedSnapshot) {
                    if (receivedSnapshot.connectionState ==
                        ConnectionState.waiting) {
                      return Center(child: CircularProgressIndicator());
                    }

                    final sentDocs = sentSnapshot.data?.docs ?? [];
                    final receivedDocs = receivedSnapshot.data?.docs ?? [];
                    final allDocs = [...sentDocs, ...receivedDocs];

                    allDocs.sort((a, b) {
                      Timestamp aTimestamp =
                          a['timestamp'] as Timestamp? ?? Timestamp.now();
                      Timestamp bTimestamp =
                          b['timestamp'] as Timestamp? ?? Timestamp.now();
                      return bTimestamp.compareTo(aTimestamp);
                    });

                    return ListView.builder(
                      reverse: true,
                      itemCount: allDocs.length,
                      itemBuilder: (ctx, index) => MessageBubble(
                        allDocs[index]['text'],
                        allDocs[index]['senderId'] == widget.senderId,
                      ),
                    );
                  },
                );
              },
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: Row(
              children: [
                Expanded(
                  child: TextField(
                    controller: _controller,
                    decoration: InputDecoration(labelText: 'Send a message...'),
                  ),
                ),
                IconButton(
                  icon: Icon(Icons.send),
                  onPressed: _sendMessage,
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class MessageBubble extends StatelessWidget {
  final String message;
  final bool isMe;

  MessageBubble(this.message, this.isMe);

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: isMe ? MainAxisAlignment.end : MainAxisAlignment.start,
      children: [
        Container(
          decoration: BoxDecoration(
            color: isMe ? Colors.grey[300] : Colors.blue[300],
            borderRadius: BorderRadius.circular(12),
          ),
          width: 140,
          padding: EdgeInsets.symmetric(vertical: 10, horizontal: 16),
          margin: EdgeInsets.symmetric(vertical: 4, horizontal: 8),
          child: Text(
            message,
            style: TextStyle(
              color: isMe ? Colors.black : Colors.white,
            ),
          ),
        ),
      ],
    );
  }
}
