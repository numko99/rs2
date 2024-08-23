import 'package:flutter/material.dart';
import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:iter_mobile/models/user_names.dart';
import 'package:iter_mobile/pages/chat/chat_page.dart';
import 'package:iter_mobile/providers/auth_storage_provider.dart';
import 'package:iter_mobile/providers/user_provider.dart';
import 'package:provider/provider.dart';

class UserListPage extends StatefulWidget {
  const UserListPage({super.key});

  @override
  _UserListPageState createState() => _UserListPageState();
}

class _UserListPageState extends State<UserListPage> {
  final String currentUserId = AuthStorageProvider.getAuthData()?["id"];

  UserProvider? _userProvider;

 Future<Map<String, UserNamesResponse>> fetchUserNames(List<String> userIds) async {
    var response = await _userProvider?.getUserNamesByIds(userIds);

    final Map<String, UserNamesResponse> userNamesMap = {
      for (var user in response!) user.id: user
    };

    return userNamesMap;
  }

  @override
  void initState() {
    super.initState();
    _userProvider = context.read<UserProvider>();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.white,
        title: Text('Razgovori'),
      ),
      body: StreamBuilder(
        stream: FirebaseFirestore.instance
            .collection('chats')
            .where('senderId', isEqualTo: currentUserId)
            .snapshots(),
        builder: (ctx, AsyncSnapshot<QuerySnapshot> snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return Center(child: CircularProgressIndicator());
          }

          final sentDocs = snapshot.data?.docs ?? [];

          return StreamBuilder(
            stream: FirebaseFirestore.instance
                .collection('chats')
                .where('receiverId', isEqualTo: currentUserId)
                .snapshots(),
            builder: (ctx, AsyncSnapshot<QuerySnapshot> receivedSnapshot) {
              if (receivedSnapshot.connectionState == ConnectionState.waiting) {
                return Center(child: CircularProgressIndicator());
              }

              final receivedDocs = receivedSnapshot.data?.docs ?? [];
              final allDocs = [...sentDocs, ...receivedDocs];

              final userMessages = <String, Map<String, dynamic>>{};

              for (var doc in allDocs) {
                String otherUserId;
                if (doc['senderId'] != currentUserId) {
                  otherUserId = doc['senderId'];
                } else {
                  otherUserId = doc['receiverId'];
                }

                Timestamp timestamp = doc['timestamp'] ?? Timestamp.now();
                if (!userMessages.containsKey(otherUserId) || ( doc['timestamp'] != null &&
                    (timestamp).compareTo(
                            userMessages[otherUserId]!['timestamp']) >
                        0)) {
                  userMessages[otherUserId] = {
                    'message': doc['text'],
                    'timestamp': doc['timestamp'],
                  };
                }
              }

              final sortedUserIds = userMessages.keys.toList();
              sortedUserIds.sort((a, b) {
                final aTimestamp = userMessages[a]!['timestamp'] as Timestamp;
                final bTimestamp = userMessages[b]!['timestamp'] as Timestamp;
                return bTimestamp.compareTo(aTimestamp);
              });

              return FutureBuilder<Map<String, UserNamesResponse>>(
              future: fetchUserNames(sortedUserIds),
              builder: (ctx, nameSnapshot) {
                if (nameSnapshot.connectionState == ConnectionState.waiting) {
                  return Center(child: CircularProgressIndicator());
                }

                if (nameSnapshot.hasError) {
                  return Center(child: Text('Error: ${nameSnapshot.error}'));
                }

                final userNames = nameSnapshot.data ?? {};

                return sortedUserIds.length == 0 
                ? Center(child: Text("Nema poruka"))
                : ListView.builder(
                  itemCount: sortedUserIds.length,
                  itemBuilder: (ctx, index) {
                    final userId = sortedUserIds[index];
                    final lastMessage =
                        userMessages[userId]!['message'] as String;
                    final lastTimestamp =
                        userMessages[userId]!['timestamp'] as Timestamp;
                    final lastDateTime = lastTimestamp.toDate();
                    final formattedTime =
                        "${lastDateTime.day}.${lastDateTime.month}.${lastDateTime.year} at ${lastDateTime.hour}:${lastDateTime.minute.toString().padLeft(2, '0')}";
                    final userName = userNames[userId]!.firstName + " " + userNames[userId]!.lastName;
                    final agencyName = userNames[userId]!.agencyName;

                    return ListTile(
                      title: Card(
                        child: Padding(
                          padding: const EdgeInsets.all(8.0),
                          child: Row(
                            children: [
                              Expanded(
                                  child: Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: [
                                      Row(
                                        mainAxisAlignment:
                                            MainAxisAlignment.spaceBetween,
                                        children: [
                                          Text(
                                              "${userName.length > 25 ? userName.substring(0, 25) : userName}",
                                              style: TextStyle(fontSize: 17)),
                                          Text(formattedTime,
                                              style: TextStyle(fontSize: 10)),
                                        ],
                                      ),
                                      Text(agencyName ?? "", style: TextStyle(fontSize: 10)),
                                      SizedBox(height: 10),
                                      Text(
                                          "${lastMessage.length > 15 ? lastMessage.substring(0, 15) + '...' : lastMessage}",
                                          style: TextStyle(fontSize: 12)),
                                    ],
                                  ),
                                ),
                            ],
                          ),
                        ),
                      ),
                      onTap: () {
                        Navigator.of(context).push(
                          MaterialPageRoute(
                            builder: (context) => ChatPage(
                              senderId: currentUserId,
                              receiverId: userId,
                              reciverAgency: agencyName ?? "",
                              reciverName: userName,
                            ),
                          ),
                        );
                      },
                    );
                  },
                );
              },
            );
            },
          );
        },
      ),
    );
  }
}
