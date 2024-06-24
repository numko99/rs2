import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_stripe/flutter_stripe.dart';
import 'package:iter_mobile/helpers/http_overrides.dart';
import 'package:iter_mobile/pages/login.dart';
import 'package:iter_mobile/providers/arrangment_provider.dart';
import 'package:iter_mobile/providers/auth_provider.dart';
import 'package:iter_mobile/providers/dropdown_provider.dart';
import 'package:iter_mobile/providers/employee_arrangment_provider.dart';
import 'package:iter_mobile/providers/reservation_provider.dart';
import 'package:iter_mobile/providers/user_provider.dart';
import 'package:provider/provider.dart';

void main() {
    HttpOverrides.global = MyHttpOverrides();
    Stripe.publishableKey = 'pk_test_51PQwmlBosSiX3Jj5zh5WMM48KhAoi2jYvFrZp1RWL7KZq6rJMgVeMcO2XoVMTE3OB5HZT95ZSv8XFoHzBHxmDQ1B000kN7XgrH';

  runApp(MultiProvider(providers: [
    ChangeNotifierProvider(create: (_) => AuthProvider()),
    ChangeNotifierProvider(create: (_) => ArrangmentProvider()),
    ChangeNotifierProvider(create: (_) => ReservationProvider()),
    ChangeNotifierProvider(create: (_) => DropdownProvider()),
    ChangeNotifierProvider(create: (_) => UserProvider()),
    ChangeNotifierProvider(create: (_) => EmployeeArrangmentProvider())
  ], child: const IterMobileApp()));
}


class IterMobileApp extends StatelessWidget {
  const IterMobileApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Iter Mobile App',
      theme: ThemeData(
        primarySwatch: Colors.amber,
      ),
      home: Login(),
    );
  }
}
