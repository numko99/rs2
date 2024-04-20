import 'package:flutter/material.dart';
import 'package:iter_mobile/pages/home.dart';
import 'package:iter_mobile/pages/profile.dart';
import 'package:iter_mobile/pages/search.dart';

class Layout extends StatefulWidget {
  const Layout({super.key});

  @override
  State<Layout> createState() => _LayoutState();
}


class _LayoutState extends State<Layout> {
  int _selectedIndex = 0; // Varijabla koja prati trenutno odabrani tab

  // Lista widgeta koji predstavljaju stranice za svaki tab
  final List<Widget> _pages = [
    HomePage(), // Stranica za prvi tab
    SearchPage(), // Stranica za drugi tab
    ProfilePage(), // Stranica za treći tab
  ];

  void _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index; // Ažuriranje indeksa na temelju odabranog taba
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: _pages.elementAt(
            _selectedIndex),
      ),
      bottomNavigationBar: BottomNavigationBar(
        items: const <BottomNavigationBarItem>[
          BottomNavigationBarItem(
            icon: Icon(Icons.home),
            label: 'Home',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.search),
            label: 'Search',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.person),
            label: 'Profile',
          ),
        ],
        currentIndex: _selectedIndex,
        onTap: _onItemTapped,
      ),
    );
  }
}
