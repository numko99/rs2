import 'package:flutter/material.dart';
import 'package:iter_mobile/enums/roles.dart';
import 'package:iter_mobile/pages/chat/chat_page.dart';
import 'package:iter_mobile/pages/chat/all_messages_page.dart';
import 'package:iter_mobile/pages/arrangements/employee_previous_arrangements_page.dart';
import 'package:iter_mobile/pages/arrangements/employee_home_page.dart';
import 'package:iter_mobile/pages/arrangements/home_page.dart';
import 'package:iter_mobile/pages/reservations/my_arrangements_page.dart';
import 'package:iter_mobile/pages/my_profile/profile_page.dart';
import 'package:iter_mobile/providers/auth_storage_provider.dart';

class Layout extends StatefulWidget {
  const Layout({super.key});

  @override
  State<Layout> createState() => _LayoutState();
}

class _LayoutState extends State<Layout> {
  int _selectedIndex = 0;

  final List<WidgetBuilder> _pages = [
    (context) => HomePage(),
    (context) => UserListPage(),
    (context) => const MyArrangementsPage(),
    (context) => ProfilePage(),
  ];

    final List<WidgetBuilder> _employePages = [
    (context) => const EmployeeHomePage(),
    (context) => UserListPage(),
    (context) => const EmployeePreviousArrangementsPage(),
    (context) => ProfilePage(),
  ];

  void _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child:AuthStorageProvider.getAuthData()?["role"] == Roles.client
              ? _pages.elementAt(_selectedIndex)(context)
              : _employePages.elementAt(_selectedIndex)(context) ,
      ),
      bottomNavigationBar: BottomNavigationBar(
        items: const <BottomNavigationBarItem>[
          BottomNavigationBarItem(
            icon: Icon(Icons.home),
            label: 'Početna',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.message),
            label: 'Poruke',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.list_alt),
            label: 'Putovanja',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.person),
            label: 'Profil',
          ),
        ],
        currentIndex: _selectedIndex,
        selectedItemColor: Colors.amber, // Boja za odabrane stavke
        unselectedItemColor: Colors.grey, // Boja za neodabrane stavke
        backgroundColor: Colors.white, // Boja pozadine
        onTap: _onItemTapped,
        type: BottomNavigationBarType.fixed, // Osigurava da je layout fiksiran
      ),
    );
  }
}
