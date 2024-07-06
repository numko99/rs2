import 'package:flutter/material.dart';
import 'package:iter_mobile/enums/dropdown_types.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/models/dropdown_model.dart';
import 'package:iter_mobile/pages/reservation_details_page.dart';
import 'package:iter_mobile/providers/dropdown_provider.dart';
import 'package:iter_mobile/providers/reservation_provider.dart';
import 'package:provider/provider.dart';

class ReservationConfirmationDialog extends StatefulWidget {
  const ReservationConfirmationDialog({super.key, required this.arrangementId});

  final String arrangementId;

  @override
  State<ReservationConfirmationDialog> createState() =>
      _ReservationConfirmationDialogState();
}

class _ReservationConfirmationDialogState
    extends State<ReservationConfirmationDialog> {

  final _formKey = GlobalKey<FormState>();

  final TextEditingController departurePlaceController = TextEditingController();
  final TextEditingController reminderController = TextEditingController();

  late ReservationProvider reservationProvider;
  late DropdownProvider dropdownProvider;
  List<DropdownModel>? prices;
  String? selectedAccomodationType;

  bool _displayLoader = true;

  @override
  void initState() {
    super.initState();
    reservationProvider = context.read<ReservationProvider>();
    dropdownProvider = context.read<DropdownProvider>();

    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      _displayLoader = true;
    });
    var usersDropdownTemp = await dropdownProvider!
        .get({"dropdownType": DropdownTypes.clients.index});

    usersDropdownTemp.result
        .insert(0, DropdownModel(id: null, name: "Nije odabrano"));

    var arrangementPricesTemp = await dropdownProvider!.get({
      "dropdownType": DropdownTypes.accomodationTypes.index,
      "arrangementId": widget.arrangementId
    });

    arrangementPricesTemp.result
        .insert(0, DropdownModel(id: null, name: "Nije odabrano"));
    setState(() {
      _displayLoader = false;
      prices = arrangementPricesTemp.result;
    });
  }

  void submit() async {
    if (_formKey.currentState?.validate() == false) {
      return;
    }

    selectedAccomodationType ??= prices![1].id;

    Map<dynamic, dynamic> request = {
      "reminder": reminderController.text,
      "departurePlace": departurePlaceController.text,
      "arrangmentId": widget.arrangementId,
      "arrangementPriceId": selectedAccomodationType
    };
    setState(() {
      _displayLoader = true;
    });
    
    try {
      var reservation = await reservationProvider.insert(request);
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Uspješno ste rezervisali aranzman",
          backgroundColor: Colors.green);

      await Navigator.of(context).push(
        MaterialPageRoute(
            builder: (context) =>
                ReservationDetailsScreen(reservationId: reservation?.id)),
      );
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške: ${error.toString()}",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        _displayLoader = true;
      });
      if (mounted) {
        Navigator.of(context).pop(true);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_displayLoader) {
      return const Center(
          child: Column(children: [CircularProgressIndicator()]));
    } else {
      return Form(
        key: _formKey,
        child: Container(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            mainAxisSize: MainAxisSize.min,
            children: <Widget>[
              Text("DODATNE INFORMACIJE O REZERVACIJI",
                  style: TextStyle(
                      color: Colors.grey[600],
                      fontSize: 14,
                      fontWeight: FontWeight.bold)),
              Divider(
                height: 15,
                color: Colors.grey[300],
                thickness: 1,
              ),
              TextFormField(
                  controller: departurePlaceController,
                  decoration: const InputDecoration(labelText: 'Grad polaska'),
                  validator: (value) {
                    if (value == null || value.isEmpty) {
                      return 'Polje  je obavezno';
                    }
                    return null;
                  }),
              const SizedBox(height: 10),
              if ((prices!.length == 2 && prices?[1].name == null) == false) ...[
                Row(
                  children: [
                    Expanded(
                      flex: 1,
                      child: DropdownButtonFormField<dynamic?>(
                          decoration: const InputDecoration(
                            labelText: 'Izaberite tip smještaja',
                          ),
                          value: selectedAccomodationType,
                          items: prices?.map((DropdownModel item) {
                            return DropdownMenuItem<dynamic>(
                              value: item.id,
                              child: Text(item.name ?? ""),
                            );
                          }).toList(),
                          validator: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Polje je obavezno';
                            }
                            return null;
                          },
                          onChanged: (value) {
                            setState(() {
                              selectedAccomodationType = value;
                            });
                          }),
                    ),
                  ],
                ),
              ],
              const SizedBox(height: 10),
              TextFormField(
                controller: reminderController,
                maxLines: 5,
                minLines: 1,
                decoration: const InputDecoration(labelText: 'Napomena'),
              ),
              const SizedBox(height: 20),
              ElevatedButton(
                onPressed: () {
                  submit();
                },
                child:
                    const Text("Potvrdi", style: TextStyle(color: Colors.white)),
              ),
            ],
          ),
        ),
      );
    }
  }
}
