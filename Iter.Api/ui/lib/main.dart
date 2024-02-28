import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/pages/agency_details.dart';
import 'package:ui/pages/arrangement_details.dart';
import 'package:ui/pages/arrangement_index.dart';
import 'package:ui/pages/arrangments_add_edit.dart';
import 'package:ui/pages/login.dart';
import 'package:ui/pages/reservation_index.dart';
import 'package:ui/pages/user_details.dart';
import 'package:ui/pages/users_index.dart';
import 'package:ui/services/agency_provider.dart';

import 'package:ui/pages/agency.dart';
import 'package:ui/services/arrangment_provider.dart';
import 'package:ui/services/auth_provider.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/services/reservation_provider.dart';
import 'package:ui/services/user_provider.dart';

void main() {
  runApp(MultiProvider(providers: [
    ChangeNotifierProvider(create: (_) => AgencyProvider()),
    ChangeNotifierProvider(create: (_) => AuthProvider()),
    ChangeNotifierProvider(create: (_) => ArrangmentProvider()),
    ChangeNotifierProvider(create: (_) => DropdownProvider()),
    ChangeNotifierProvider(create: (_) => UserProvider()),
    ChangeNotifierProvider(create: (_) => ReservationProvider()),
  ], child: IterApp()));
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
            return ArrangementIndexPage();
          },
          '/arrangement/addEdit': (context) {
            final args = ModalRoute.of(context)!.settings.arguments as Map;
            return ArrangementAddEditPage(agencyId: args['agencyId'], arrangementId: args["arrangementId"]);
          },
          '/arrangement/details': (context) {
            final args = ModalRoute.of(context)!.settings.arguments as Map;
            return ArrangementDetailsPage(id: args["id"]);
          },
           '/reservations': (context) {
            return ReservationIndexPage();
          },
        });
  }
}
