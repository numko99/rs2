import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:ui/widgets/Layout/sidebarItem.dart';
import 'package:ui/widgets/logo.dart';

class Layout extends StatelessWidget {
  final Widget body;

  Layout({required this.body});

  var sidebarItems = [
    {
      'text': 'Početna',
      'icon': Icons.home,
      'link': '/home',
    },
    {
      'text': 'Korisnici',
      'icon': Icons.person,
      'link': '/users',
    },
    {
      'text': 'Agencije',
      'icon': Icons.card_travel,
      'link': '/agency',
    },
    {
      'text': 'Izvještaji',
      'icon': Icons.description,
      'link': '/agency',
    },
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.white,
        iconTheme: const IconThemeData(
          color: Colors.amber,
        ),
      ),
      body: Container(
        color: CupertinoColors.lightBackgroundGray,
        child: Column(
          children: <Widget>[
            const SizedBox(height: 40),
            Container(
              child: const Card(
                child: Padding(
                  padding: EdgeInsets.fromLTRB(50, 0, 50, 0),
                  child: Row(
                    children: [
                      Text(
                        "Agencije",
                        style: TextStyle(color: Colors.amber, fontSize: 30),
                      ),
                      Expanded(
                        child: Align(
                          alignment: Alignment.centerRight,
                          child: Icon(
                            Icons.home,
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
            Expanded(child: SingleChildScrollView(child: body))
          ],
        ),
      ),
      drawer: Drawer(
          child: Column(
        children: <Widget>[
          Expanded(
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
              child: Align(
            alignment: Alignment.bottomCenter,
            child: SidebarItem(
              text: "Odjava",
              icon: Icons.logout,
              link: "/logout",
            ),
          ))
        ],
      )),
    );
  }
}
