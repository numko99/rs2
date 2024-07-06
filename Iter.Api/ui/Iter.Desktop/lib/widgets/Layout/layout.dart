import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:ui/enums/roles.dart';
import 'package:ui/services/auth_storage_provider.dart';
import 'package:ui/widgets/Layout/sidebarItem.dart';
import 'package:ui/widgets/logo.dart';

class Layout extends StatefulWidget {
  const Layout({
    super.key,
    required this.body,
    required this.name,
    required this.icon,
  });

  final Widget body;
  final String name;
  final IconData icon;

  @override
  State<Layout> createState() => _LayoutState();
}

class _LayoutState extends State<Layout> {
  Roles? role;
  @override
  void initState() {
    super.initState();

    role = AuthStorageProvider.getAuthData()?["role"];

    sidebarItems = sidebarItems.where((item) {
      var roles = item['roles'] as List<Roles>?;
      return roles?.contains(role) ?? false;
    }).toList();
  }

  var sidebarItems = [
    {
      'text': 'Početna',
      'icon': Icons.home,
      'link': '/home',
      'roles': [Roles.admin]
    },
    {
      'text': 'Korisnici',
      'icon': Icons.beach_access_outlined,
      'link': '/users',
      'roles': [Roles.admin, Roles.coordinator]
    },
    {
      'text': 'Agencije',
      'icon': Icons.business,
      'link': '/agency',
      'roles': [Roles.admin]
    },
    {
      'text': 'Aranžmani',
      'icon': Icons.beach_access_outlined,
      'link': '/arrangements',
      'roles': [Roles.admin, Roles.coordinator]
    },
    {
      'text': 'Rezervacije',
      'icon': Icons.list_alt,
      'link': '/reservations',
      'roles': [Roles.admin, Roles.coordinator]
    },
    {
      'text': 'Izvještaji',
      'icon': Icons.description,
      'link': '/reports',
      'roles': [Roles.admin, Roles.coordinator]
    },
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.white,
        iconTheme: const IconThemeData(color: Colors.amber),
      ),
      body: Container(
        color: CupertinoColors.lightBackgroundGray,
        child: Column(
          children: <Widget>[
            const SizedBox(height: 40),
            Container(
              child: Card(
                child: Padding(
                  padding: EdgeInsets.fromLTRB(50, 0, 50, 0),
                  child: Row(
                    children: [
                      IconButton(
                        color: Colors.amber,
                        icon: Icon(Icons.arrow_back),
                        onPressed: () {
                          if (Navigator.canPop(context)) {
                            Navigator.pop(context);
                          } else {}
                        },
                      ),
                      SizedBox(width: 20),
                      Text(
                        widget.name,
                        style: TextStyle(color: Colors.amber, fontSize: 30),
                      ),
                      Expanded(
                        child: Align(
                          alignment: Alignment.centerRight,
                          child: Icon(
                            widget.icon,
                            color: Colors.amber,
                            size: 70.0,
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
            const SizedBox(height: 20),
            Expanded(child: SingleChildScrollView(child: widget.body))
          ],
        ),
      ),
      drawer: Drawer(
          child: Column(
        children: <Widget>[
          Expanded(
            flex: 3,
            child: ListView(
              children: <Widget>[
                Container(
                  decoration: BoxDecoration(
                    boxShadow: [
                      BoxShadow(
                        color: Colors.black.withOpacity(0.2),
                        offset: const Offset(0, 3),
                        blurRadius: 4,
                      ),
                    ],
                  ),
                  child: Row(
                    children: <Widget>[
                      Expanded(
                        child: Container(
                          color: Colors.amber,
                          child: Center(
                            child: Logo(fontSize: 46.6, color: Colors.white),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 30),
                for (var item in sidebarItems)
                  SidebarItem(
                      text: item['text'] as String,
                      icon: item['icon'] as IconData,
                      link: item['link'] as String),
              ],
            ),
          ),
          Expanded(
              flex: 1,
              child: Align(
                alignment: Alignment.bottomCenter,
                child: SidebarItem(
                  text: "Odjava",
                  icon: Icons.logout,
                  link: "/login",
                ),
              ))
        ],
      )),
    );
  }
}
