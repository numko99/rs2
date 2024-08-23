import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_stripe/flutter_stripe.dart';
import 'package:iter_mobile/apiConfig.dart';
import 'package:iter_mobile/helpers/http_overrides.dart';
import 'package:iter_mobile/pages/sign_up/login_page.dart';
import 'package:iter_mobile/providers/arrangment_provider.dart';
import 'package:iter_mobile/providers/auth_provider.dart';
import 'package:iter_mobile/providers/dropdown_provider.dart';
import 'package:iter_mobile/providers/employee_arrangment_provider.dart';
import 'package:iter_mobile/providers/reservation_provider.dart';
import 'package:iter_mobile/providers/user_provider.dart';
import 'package:provider/provider.dart';
import 'package:firebase_core/firebase_core.dart';

void main() async {
  HttpOverrides.global = MyHttpOverrides();
  Stripe.publishableKey = const String.fromEnvironment('STRIPE_KEY', defaultValue: ApiConfig.StripeKey);
  WidgetsFlutterBinding.ensureInitialized();

  
  await Firebase.initializeApp(
    options: const FirebaseOptions(
      apiKey: String.fromEnvironment('API_KEY', defaultValue: ApiConfig.FireBaseApiKey),
      appId: String.fromEnvironment('APP_ID', defaultValue: ApiConfig.FirebaseAppId),
      messagingSenderId: String.fromEnvironment('MESSEGING_SENDER_ID', defaultValue: ApiConfig.FirebaseMessagingSenderId),
      projectId: String.fromEnvironment('FIREBASE_PROJECT', defaultValue: ApiConfig.FirebaseProjectId)
    ),
  );

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
        useMaterial3: false,
        primarySwatch: Colors.amber,
        primaryColor: Colors.amber,
      ),
      home: const Login(),
    );
  }
}
