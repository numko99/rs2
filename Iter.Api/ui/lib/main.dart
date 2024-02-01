import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/pages/agency_details.dart';
import 'package:ui/pages/arrangments_add_edit.dart';
import 'package:ui/services/agency_provider.dart';

import 'package:ui/pages/agency.dart';
import 'package:ui/services/arrangment_provider.dart';
import 'package:ui/services/auth_provider.dart';
import 'package:ui/services/dropdown_provider.dart';

void main() {
  runApp(MultiProvider(providers: [
    ChangeNotifierProvider(create: (_) => AgencyProvider()),
    ChangeNotifierProvider(create: (_) => AuthProvider()),
    ChangeNotifierProvider(create: (_) => ArrangmentProvider()),
    ChangeNotifierProvider(create: (_) => DropdownProvider()),
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
          '/': (context) => const ArrangementAddEditPage(),
          '/agency': (context) => const AgencyPage(),
          '/agency/details': (context) {
            final args = ModalRoute.of(context)!.settings.arguments as Map;
            return AgencyDetailsPage(id: args['id']);
          },
        });
  }
}
