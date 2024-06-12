
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/helpers/dateTime_helper.dart';
import 'package:ui/models/user.dart';
import 'package:ui/services/user_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';
import 'package:ui/widgets/reservation/reservation_mini_data_table.dart';

class UserDetailsPage extends StatefulWidget {
  const UserDetailsPage({super.key, required this.id});

  final String id;

  @override
  State<UserDetailsPage> createState() => _UserDetailsPageState();
}

class _UserDetailsPageState extends State<UserDetailsPage> {
  UserProvider? _userProvider;

  User? user;
  bool displayLoader = true;

  @override
  void initState() {
    super.initState();
    _userProvider = context.read<UserProvider>();
    loadData();
  }

  Future<void> loadData() async {
    setState(() {
      displayLoader = true;
    });

    var userTemp = await _userProvider?.getById(widget.id);
    setState(() {
      user = userTemp;
      displayLoader = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      name: "Korisnici",
      icon: Icons.person_2,
      body: displayLoader
          ? const Center(child: Column(children: [CircularProgressIndicator()]))
          : Card(
              child: Padding(
                padding: const EdgeInsets.fromLTRB(45.0, 10, 45.0, 10),
                child: Column(
                  children: [
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Expanded(
                            flex: 2,
                            child: Container(
                              child: _buildAgencyDetails(user),
                            )),
                        Expanded(flex: 3, child: ReservationDataTable(userId: widget.id)),
                      ],
                    ),
                  ],
                ),
              ),
            ),
    );
  }

  Widget _buildAgencyDetails(User? user) {
    return Column(
      children: [
        const SizedBox(height: 15),
        Center(
            child: Text(
          "${user!.firstName!} ${user.lastName!}",
          style: const TextStyle(fontSize: 20),
        )),
        const SizedBox(height: 20),
        Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Datum rođenja',
                  child: Icon(Icons.calendar_month, color: Colors.amber),
                ),
                title: Text(DateTimeHelper.formatDate(user.birthDate, "dd.MM.yyyy.")),
              ),
            ),
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Mjesto prebivališta',
                  child: Icon(Icons.location_city, color: Colors.amber),
                ),
                title: Text(user.residencePlace),
              ),
            ),
          ],
        ),
        Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Email',
                  child: Icon(Icons.email, color: Colors.amber),
                ),
                title: Text(user?.email ?? ''),
              ),
            ),
            Flexible(
                child: ListTile(
              leading: const Tooltip(
                message: 'Broj telefona',
                child: Icon(Icons.phone, color: Colors.amber),
              ),
              title: Text(user?.phoneNumber ?? ''),
            )),
          ],
        ),
      ],
    );
  }
}
