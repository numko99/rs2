
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/roles.dart';
import 'package:ui/helpers/dateTime_helper.dart';
import 'package:ui/models/user.dart';
import 'package:ui/services/user_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';
import 'package:ui/widgets/employee_arrangements_data_table.dart';
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
                    Card(child: Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: _buildAgencyDetails(user),
                    )),
                    const SizedBox(height: 40),
                    Card(child: Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: Column(
                        children: [
                          const Align(alignment: Alignment.centerLeft, child: Text("Rezervacije", style: TextStyle(fontSize: 20))),
                          const SizedBox(height: 20),
                          if (user!.role == (Roles.client.index + 1))
                          ReservationDataTable(userId: widget.id),
                          if (user!.role == (Roles.touristGuide.index + 1))
                            EmployeeArrangementsDataTable(employeeId: user!.employeeId),
                        ],
                      ),
                    ))
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
        Align(
          alignment: Alignment.centerLeft,
          child: Column(
            children: [
          Text(
            "${user!.firstName!} ${user.lastName!}",
            style: const TextStyle(fontSize: 20),
          ),
          if (user.role == (Roles.touristGuide.index + 1) ||
              user.role == (Roles.coordinator.index + 1))
            Padding(
              padding: const EdgeInsets.only(left: 18.0),
              child: Text(user.agency!.name, style: const TextStyle(fontSize: 15)),
            )
            ],
          ),
        ),
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
            Flexible(
              child: ListTile(
                leading: Tooltip(
                  message: 'Korisnik aktivan',
                  child: user.isActive == true ? const Icon(Icons.check, color: Colors.green) : const Icon(Icons.close, color: Colors.red),
                ),
                title: Text(user.isActive == true ? "Aktivan" : 'Neaktivan'),
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
                title: Text(user.email ?? ''),
              ),
            ),
            Flexible(
                child: ListTile(
              leading: const Tooltip(
                message: 'Broj telefona',
                child: Icon(Icons.phone, color: Colors.amber),
              ),
              title: Text(user.phoneNumber ?? ''),
            )),
             Flexible(
                child: ListTile(
                leading: const Tooltip(
                message: 'Tip korisnika',
                child: Icon(Icons.person, color: Colors.amber),
              ),
              title: Text(RoleEnumManager.getRoleNamesById(user.role)),
            )),
          ],
        ),
      ],
    );
  }
}
