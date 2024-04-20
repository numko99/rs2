import 'package:flutter/material.dart';

class HomePage extends StatefulWidget {
  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  final List<String> dummyData = [
    'Dummy Data 1',
    'Dummy Data 2',
    'Dummy Data 3',
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
       appBar: AppBar(
        title: Icon(Icons.home),
      ),
      body: ListView.builder(
        itemCount: dummyData.length,
        itemBuilder: (context, index) {
          return Card(
            child: ListTile(
              title: Text(dummyData[index]),
            ),
          );
        },
      ),
    );
  }
}
