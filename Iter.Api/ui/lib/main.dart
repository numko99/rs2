import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:injectable/injectable.dart';
import 'main.config.dart';

import 'package:ui/pages/home.dart';
import 'package:ui/pages/login.dart';

final getIt = GetIt.instance;

@injectableInit
void configureDependencies() => $initGetIt(getIt);

void main() {
  configureDependencies();
  runApp(IterApp());
}

class IterApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'ITer',
      theme: ThemeData(
        primarySwatch: Colors.amber,
        fontFamily: 'Elsie-Regular',
      ),
      initialRoute: '/',
      routes: {
        '/': (context) => Login(),
        '/home': (context) => Home(),
      },
    );
  }
}
