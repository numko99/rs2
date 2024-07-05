import 'package:flutter/material.dart';
import 'package:iter_mobile/enums/roles.dart';
import 'package:iter_mobile/models/user_statistic_data.dart';
import 'package:iter_mobile/pages/edit_profile.dart';
import 'package:iter_mobile/pages/password_change.dart';
import 'package:iter_mobile/providers/auth_provider.dart';
import 'package:iter_mobile/providers/auth_storage_provider.dart';
import 'package:iter_mobile/providers/user_provider.dart';
import 'package:provider/provider.dart';

class ProfilePage extends StatefulWidget {
  @override
  _ProfilePageState createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  bool displayLoader = true;
  UserProvider? _userProvider;
  AuthProvider? _authProvider;
  UserStatisticData? user;

  @override
  void initState() {
    super.initState();

    _userProvider = context.read<UserProvider>();
    _authProvider = context.read<AuthProvider>();
    loadData();
  }

  void loadData() async {
    var userTemp = await _userProvider?.getCurrentUserStatisticData();

    setState(() {
      displayLoader = false;
      user = userTemp;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        toolbarHeight: 0,
      ),
      body: displayLoader
          ? const Center(child: CircularProgressIndicator())
          : ListView(
              padding: const EdgeInsets.all(20.0),
              children: <Widget>[
                const SizedBox(height: 20),
                const Center(
                    child: Icon(Icons.person, size: 100, color: Colors.grey)),
                Center(
                    child: Text("${user?.firstName} ${user?.lastName}",
                        style: const TextStyle(fontSize: 20))),
                const SizedBox(height: 10),
                Card(
                  child: Padding(
                    padding: EdgeInsets.all(16.0),
                    child: Row(
                      crossAxisAlignment: CrossAxisAlignment.end,
                      children: <Widget>[
                        Expanded(
                          child: ListTile(
                            title: Center(
                                child:
                                    Text(user!.arrangementsCount.toString())),
                            subtitle: Center(child: Text("Broj putovanja")),
                          ),
                        ),
                        if (AuthStorageProvider.getAuthData()?["role"] ==
                            Roles.client)
                          Expanded(
                            child: ListTile(
                              title: Center(
                                  child:
                                      Text(user!.reservationCount.toString())),
                              subtitle: Center(child: Text("Broj rezervacija")),
                            ),
                          ),
                          if (AuthStorageProvider.getAuthData()?["role"] ==
                            Roles.touristGuide)
                        Expanded(
                            child: ListTile(
                          title: Center(
                              child: Text(user!.avgRating != 0 ? user!.avgRating.toString() : "-")),
                          subtitle: Center(child: Text("Prosječna ocjena")),
                        )),
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: 20),
                GestureDetector(
                    onTap: () async {
                      final isSuccess = await Navigator.of(context).push(
                        MaterialPageRoute(
                            builder: (context) => const EditProfilePage()),
                      );
                      if (isSuccess == true) loadData();
                    },
                    child: const Card(
                      child: ListTile(
                          title: Text('Uredi profil'),
                          leading: Icon(Icons.edit_note_sharp)),
                    )),
                Card(
                  child: ExpansionTile(
                    leading: const Icon(Icons.settings),
                    title: const Text("Postavke"),
                    children: <Widget>[
                      ListTile(
                        leading: const Icon(Icons.password),
                        title: const Text("Promijeni lozinku"),
                        onTap: () async {
                          final isSuccess = await Navigator.of(context).push(
                              MaterialPageRoute(
                                  builder: (context) => ChangePasswordPage()));
                          if (isSuccess == true) loadData();
                        },
                      ),
                      // ListTile(
                      //   leading: const Icon(Icons.delete_forever),
                      //   title: const Text("Obriši račun"),
                      //   onTap: () {
                      //     // Dodajte akciju za brisanje računa
                      //   },
                      // ),
                      ListTile(
                        leading: const Icon(Icons.logout),
                        title: const Text("Odjava"),
                        onTap: () {
                          showDialog(
                            context: context,
                            builder: (BuildContext context) {
                              return AlertDialog(
                                title: const Text('Potvrda'),
                                content: const Text(
                                    'Da li ste sigurni da želite da se odjavite?'),
                                actions: <Widget>[
                                  TextButton(
                                    onPressed: () {
                                      Navigator.of(context).pop();
                                    },
                                    child: const Text('Odustani'),
                                  ),
                                  TextButton(
                                    onPressed: () {
                                      Navigator.of(context).pop();
                                      _authProvider?.logoutUserAsync(context);
                                    },
                                    child: const Text('Potvrdi'),
                                  ),
                                ],
                              );
                            },
                          );
                        },
                      ),
                    ],
                  ),
                ),
              ],
            ),
    );
  }
}
