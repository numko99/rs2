import 'dart:io';

import 'package:firedart/firestore/firestore.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/apiConfig.dart';
import 'package:ui/pages/agency_details_page.dart';
import 'package:ui/pages/arrangement_details_page.dart';
import 'package:ui/pages/arrangement_index_page.dart';
import 'package:ui/pages/arrangments_add_edit_page.dart';
import 'package:ui/pages/employee_home_page.dart';
import 'package:ui/pages/profile_edit_page.dart';
import 'package:ui/pages/admin_home_page.dart';
import 'package:ui/pages/login_page.dart';
import 'package:ui/pages/messages/all_messages_page.dart';
import 'package:ui/pages/reports/agency_earnings_report.dart';
import 'package:ui/pages/reports/user_payments_report.dart';
import 'package:ui/pages/reports_index_page.dart';
import 'package:ui/pages/reservation_index_page.dart';
import 'package:ui/pages/settings_page.dart';
import 'package:ui/pages/user_details_page.dart';
import 'package:ui/pages/users_index_page.dart';
import 'package:ui/services/agency_provider.dart';

import 'package:ui/pages/agency_index_page.dart';
import 'package:ui/services/arrangment_provider.dart';
import 'package:ui/services/auth_provider.dart';
import 'package:ui/services/city_provider.dart';
import 'package:ui/services/country_provider.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/services/employee_arrangment_provider.dart';
import 'package:ui/services/report_provider.dart';
import 'package:ui/services/reservation_provider.dart';
import 'package:ui/services/statistic_provider.dart';
import 'package:ui/services/user_provider.dart';
import 'package:window_size/window_size.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  if (Platform.isWindows || Platform.isLinux || Platform.isMacOS) {
    setWindowMinSize(const Size(1600, 800));
  }

  Firestore.initialize(String.fromEnvironment('FIREBASE_PROJECT', defaultValue: ApiConfig.FirebaseProjectId));
  // await Firebase.initializeApp(
  //   options: const FirebaseOptions(
  //       apiKey: String.fromEnvironment('API_KEY', defaultValue: ApiConfig.FireBaseApiKey),
  //       authDomain: String.fromEnvironment('AUTH_DOMAIN',
  //           defaultValue: ApiConfig.FirebaseAuthDomain),
  //       appId: String.fromEnvironment('APP_ID',
  //           defaultValue: ApiConfig.FirebaseAppId),
  //         storageBucket: String.fromEnvironment('STORAGE_BUCKET',
  //           defaultValue: ApiConfig.FirebaseStorageBucket),
  //       messagingSenderId: String.fromEnvironment('MESSEGING_SENDER_ID',
  //           defaultValue: ApiConfig.FirebaseMessagingSenderId),
  //       projectId: String.fromEnvironment('FIREBASE_PROJECT',
  //           defaultValue: ApiConfig.FirebaseProjectId)),
  // );

  runApp(MultiProvider(providers: [
    ChangeNotifierProvider(create: (_) => AgencyProvider()),
    ChangeNotifierProvider(create: (_) => AuthProvider()),
    ChangeNotifierProvider(create: (_) => ArrangmentProvider()),
    ChangeNotifierProvider(create: (_) => DropdownProvider()),
    ChangeNotifierProvider(create: (_) => UserProvider()),
    ChangeNotifierProvider(create: (_) => ReservationProvider()),
    ChangeNotifierProvider(create: (_) => ReportProvider()),
    ChangeNotifierProvider(create: (_) => EmployeeArrangmentProvider()),
    ChangeNotifierProvider(create: (_) => StatisticProvider()),
    ChangeNotifierProvider(create: (_) => CityProvider()),
    ChangeNotifierProvider(create: (_) => CountryProvider())
  ], child: IterApp()));
}

class IterApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
        title: 'ITer',
        theme: ThemeData(
          useMaterial3: false,
          primarySwatch: Colors.amber,
          primaryColor: Colors.amber,
          fontFamily: 'Elsie-Regular',
        ),
        initialRoute: '/login',
        routes: {
          '/login': (context) => Login(),
          '/home': (context) => Home(),
          '/employeeHome': (context) => EmployeeHome(),
          '/agency': (context) => const AgencyPage(),
          '/agency/details': (context) {
            final args = ModalRoute.of(context)!.settings.arguments as Map;
            return AgencyDetailsPage(id: args['id']);
          },
          '/users': (context) => const UsersIndexPage(),
          '/user/details': (context) {
            final args = ModalRoute.of(context)!.settings.arguments as Map;
            return UserDetailsPage(id: args['id']);
          },
          '/arrangements': (context) {
            return const ArrangementIndexPage();
          },
          '/arrangement/addEdit': (context) {
            final args = ModalRoute.of(context)!.settings.arguments as Map;
            return ArrangementAddEditPage(
                agencyId: args['agencyId'],
                arrangementId: args["arrangementId"]);
          },
          '/arrangement/details': (context) {
            final args = ModalRoute.of(context)!.settings.arguments as Map;
            return ArrangementDetailsPage(id: args["id"]);
          },
          '/report/userPayments': (context) {
            final args = ModalRoute.of(context)!.settings.arguments as Map;
            return UserPaymentReportPage(
                arrangementId: args["id"],
                arrangementName: args["name"],
                dateFrom: args["dateFrom"],
                dateTo: args["dateTo"]);
          },
          '/report/arrangementEarnings': (context) {
            final args = ModalRoute.of(context)!.settings.arguments as Map;
            return AgencyEarningsReportPage(
                agencyId: args["id"],
                agencyName: args["name"],
                dateFrom: args["dateFrom"],
                dateTo: args["dateTo"]);
          },
          '/reservations': (context) {
            return const ReservationIndexPage();
          },
          '/reports': (context) {
            return const ReportsIndexPage();
          },
           '/my-profile': (context) {
            return const EditProfilePage();
          },
           '/settings': (context) {
            return const SettingsPage();
          },
          //  '/messages': (context) {
          //   return const UserListPage();
          // },
        });
  }
}
